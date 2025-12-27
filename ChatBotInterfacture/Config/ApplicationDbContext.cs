using ChatBotInterfacture.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Config
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // --- Khai báo DbSet ---
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentChunk> DocumentChunks { get; set; }

        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<MessageCitation> MessageCitations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Thiết lập quan hệ nhiều-nhiều giữa User và Course thông qua Enrollment
            // --- CẤU HÌNH QUAN HỆ & RÀNG BUỘC ---
            builder.HasPostgresExtension("vector");

            // 1. Enrollment: Một sinh viên không thể đăng ký 2 lần vào 1 khóa học
            builder.Entity<Enrollment>()
            .HasIndex(e => new { e.UserId, e.CourseId })
            .IsUnique();

        // 2. Cascade Delete: Xóa User -> Xóa sạch dữ liệu liên quan
        builder.Entity<User>()
            .HasMany(u => u.RefreshTokens)
            .WithOne(t => t.User)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<User>()
            .HasMany(u => u.Conservations)
            .WithOne(c => c.User)
            .OnDelete(DeleteBehavior.Cascade);

        // 3. Xóa Môn học -> Xóa các Khóa học con
        builder.Entity<Subject>()
            .HasMany(s => s.Courses)
            .WithOne(c => c.Subject)
            .OnDelete(DeleteBehavior.Cascade);

        // 4. Xóa Khóa học -> Xóa Enrollment và Tài liệu riêng của khóa đó
        builder.Entity<Course>()
            .HasMany(c => c.Enrollments)
            .WithOne(e => e.Course)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Course>()
            .HasMany(c => c.Documents)
            .WithOne(d => d.Course)
            .OnDelete(DeleteBehavior.Cascade);

        // 5. Xóa Document -> Xóa Chunks
        builder.Entity<Document>()
            .HasMany(d => d.DocumentChunks)
            .WithOne(c => c.Document)
            .OnDelete(DeleteBehavior.Cascade);
            
        // 6. Xóa Conversation -> Xóa Messages
        builder.Entity<Conversation>()
            .HasMany(c => c.Messages)
            .WithOne(m => m.Conversation)
            .OnDelete(DeleteBehavior.Cascade);

            // --- CẤU HÌNH VECTOR (Nếu dùng PostgreSQL + pgvector) ---
            // builder.HasPostgresExtension("vector");
            builder.Entity<DocumentChunk>()
                .Property(c => c.Vector)
                .HasColumnType("vector(1536)"); // 1536 là số chiều của OpenAI/Gemini embeddings
        }
    }
}
