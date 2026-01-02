using Domain.Entity;
using Domain.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// --- QUAN TRỌNG: Đặt biệt danh để tránh xung đột với System.Reflection.Metadata ---
using DocumentEntity = Domain.Entity.Document;

namespace ChatBotInterfacture.Config
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // --- KHAI BÁO DBSET ---
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        // Dùng tên Alias ở đây cho rõ ràng
        public DbSet<DocumentEntity> Documents { get; set; }
        public DbSet<DocumentChunk> DocumentChunks { get; set; }

        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<MessageCitation> MessageCitations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 1. Bắt buộc gọi base để cấu hình Identity (User, Role, Claims...)
            base.OnModelCreating(builder);

            // 2. Bật Extension Vector cho PostgreSQL (Bắt buộc cho AI)
            builder.HasPostgresExtension("vector");

            // ====================================================
            // A. CẤU HÌNH USER & AUTH
            // ====================================================

            builder.Entity<User>(entity =>
            {
                entity.ToTable("AspNetUsers"); // Hoặc tên bạn muốn
                entity.Property(u => u.FullName).HasMaxLength(100);
                entity.Property(u => u.StudentId).HasMaxLength(50);
                entity.Property(u => u.Major).HasMaxLength(100);

                // Quan hệ 1-N: User - RefreshToken
                entity.HasMany(u => u.RefreshTokens)
                      .WithOne()
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                // Cấu hình Value Object (OwnsOne)
                entity.OwnsOne(u => u.Address, address =>
                {
                    // Đặt tên cột trong DB (Tùy chọn, mặc định là Address_Street)
                    address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(200);
                    address.Property(a => a.City).HasColumnName("City").HasMaxLength(100);
                    address.Property(a => a.State).HasColumnName("State").HasMaxLength(100);
                    address.Property(a => a.Country).HasColumnName("Country").HasMaxLength(100);
                    address.Property(a => a.ZipCode).HasColumnName("ZipCode").HasMaxLength(20);
                });
                
            });

            builder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");
                entity.HasKey(rt => rt.Id);
            });

            // ====================================================
            // B. CẤU HÌNH LMS (SUBJECT - COURSE - ENROLLMENT)
            // ====================================================
            builder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subjects");
                entity.HasKey(s => s.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
                entity.Property(s => s.Code).IsRequired().HasMaxLength(50);

                // Mã môn học là duy nhất
                entity.HasIndex(s => s.Code).IsUnique();

                // Subject - Course (Xóa Subject phải kiểm tra kỹ -> Restrict)
                entity.HasMany(s => s.Courses)
                      .WithOne(c => c.Subject)
                      .HasForeignKey(c => c.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Subject - Document (Xóa Subject -> Không cho xóa nếu còn tài liệu)
                entity.HasMany(s => s.Documents)
                      .WithOne(d => d.Subject)
                      .HasForeignKey(d => d.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.HasKey(c => c.Id);

                // Lưu Enum dạng String cho dễ đọc
                entity.Property(c => c.Status).HasConversion<string>();
                entity.Property(c => c.Level).HasConversion<string>();

                // Course - Document (Xóa Course -> Xóa luôn tài liệu riêng của nó)
                entity.HasMany(c => c.Documents)
                      .WithOne(d => d.Course)
                      .HasForeignKey(d => d.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.OwnsOne(c => c.Price, price =>
                {
                    price.Property(p => p.Amount).HasColumnName("PriceAmount").IsRequired();
                    price.Property(p => p.Currency).HasColumnName("PriceCurrency").IsRequired().HasMaxLength(10);
                });
            
            });

            builder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("Enrollments");
                entity.HasKey(e => e.Id);

                // Một sinh viên không thể đăng ký 2 lần vào 1 khóa học
                entity.HasIndex(e => new { e.UserId, e.CourseId }).IsUnique();

                entity.HasOne(e => e.Student)
                      .WithMany(u => u.Enrollments)
                      .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Course)
                      .WithMany(c => c.Enrollments)
                      .HasForeignKey(e => e.CourseId);
            });

            // ====================================================
            // C. CẤU HÌNH TÀI LIỆU & VECTOR (RAG)
            // ====================================================
            builder.Entity<DocumentEntity>(entity =>
            {
                entity.ToTable("Documents");
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Title).IsRequired().HasMaxLength(200);
                entity.Property(d => d.FilePath).IsRequired();
                entity.Property(d => d.FileSize).IsRequired(); // long

                // Lưu Enum dạng String
                entity.Property(d => d.Status).HasConversion<string>();
                entity.Property(d => d.FileType).HasConversion<string>();

                // Document - Chunks (Quan trọng: Xóa Doc -> Xóa hết Vector)
                entity.HasMany(d => d.DocumentChunks)
                      .WithOne() // Không cần property ngược ở Chunk
                      .HasForeignKey(c => c.DocumentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Cấu hình Field Access Mode cho Collection (nếu dùng private list)
                var navigation = entity.Metadata.FindNavigation(nameof(DocumentEntity.DocumentChunks));
                navigation?.SetPropertyAccessMode(PropertyAccessMode.Field); // Bắt buộc dùng Field
                navigation?.SetField("_chunks"); // <--- QUAN TRỌNG: Chỉ định tên biến private
            });

            builder.Entity<DocumentChunk>(entity =>
            {
                entity.ToTable("DocumentChunks");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Content).IsRequired();

                // Cấu hình kiểu dữ liệu Vector cho PostgreSQL
                entity.Property(c => c.Vector)
                      .HasColumnType("vector(1536)");
            });

            // ====================================================
            // D. CẤU HÌNH CHATBOT
            // ====================================================
            builder.Entity<Conversation>(entity =>
            {
                entity.ToTable("Conversations");
                entity.HasKey(c => c.Id);

                // Tắt tự tăng vì ta dùng Guid.NewGuid() ở Domain
                entity.Property(c => c.Id).ValueGeneratedNever();

                entity.Property(c => c.Title).HasMaxLength(200);
                entity.Property(c => c.UserId).IsRequired();

                // Conversation - ChatMessages
                entity.HasMany(c => c.Messages)
                      .WithOne() // Không cần property ngược ở Message
                      .HasForeignKey(m => m.ConversationId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Access Mode cho Encapsulation
                entity.Metadata.FindNavigation(nameof(Conversation.Messages))
                      ?.SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            builder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("ChatMessages");
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).ValueGeneratedNever();
                entity.Property(m => m.Content).IsRequired();

                // Lưu Role (User, System, Assistant) dạng String
                entity.Property(m => m.Role).HasConversion<string>();
            });

            builder.Entity<MessageCitation>(entity =>
            {
                entity.ToTable("MessageCitations");
                entity.HasKey(mc => mc.Id);

                entity.HasOne(mc => mc.ChatMessage)
                      .WithMany()
                      .HasForeignKey(mc => mc.ChatMessageId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(mc => mc.DocumentChunk)
                      .WithMany()
                      .HasForeignKey(mc => mc.DocumentChunkId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}