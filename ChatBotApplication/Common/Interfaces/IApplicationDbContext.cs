using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ChatBotApplication.Common.Interfaces
{
    public interface IApplicationDbContext
    {
         // 1. Các bảng cũ của LMS
        DbSet<Subject> Subjects { get; }
        DbSet<Course> Courses { get; }
        DbSet<Enrollment> Enrollments { get; }
        
        // 2. Các bảng mới cho Chatbot & RAG (BẮT BUỘC THÊM VÀO ĐÂY)
        DbSet<Conversation> Conversations { get; }
        DbSet<ChatMessage> ChatMessages { get; }
        DbSet<MessageCitation> MessageCitations { get; }
        
        // Lưu ý: DocumentEntity là alias bạn đã đặt, hoặc dùng tên gốc Document
        DbSet<Domain.Entity.Document> Documents { get; } 
        DbSet<DocumentChunk> DocumentChunks { get; }

        // 3. Hàm lưu thay đổi
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}