using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ChatMessage : BaseEntities
    {

        // 1. Properties (Private Set)
        public Guid ConversationId { get; private set; } // FK là Guid
        public MessageRole Role { get; private set; }    // Dùng Enum
        public string Content { get; private set; }

        // Optional: Lưu số token để thống kê/billing
        public int TokenCount { get; private set; }

        public ChatMessage() { }
        public ChatMessage(Guid conversationId, MessageRole role, string content, int tokenCount = 0)
        {
            if (conversationId == Guid.Empty) throw new DomainException("ConversationId cannot be empty");
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentNullException(nameof(content));
            ConversationId = conversationId;
            Role = role;
            Content = content;
            TokenCount = tokenCount;
        }

        // 3. Behavior (Nếu cần sửa nội dung tin nhắn sau này)
        // Ví dụ: Bot trả lời sai, User muốn edit lại câu trả lời để Fine-tune
        public void UpdateContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent)) throw new DomainException("New Content cannot be empty");
            Content = newContent;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
