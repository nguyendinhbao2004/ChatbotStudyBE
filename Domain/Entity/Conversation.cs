
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
    public class Conversation : BaseEntities, IAggregateRoot
    {

        // 1. Properties (Private Set để bảo vệ dữ liệu)
        public string UserId { get; private set; }
        public virtual User? User { get; private set; }

        public string Title { get; private set; }

        public DateTime LastActive { get; private set; }

        //2. Encapsulation cho List Message
        // Chỉ class này mới được thêm/sửa list này
        private readonly List<ChatMessage> _messages = new();
        // Bên ngoài chỉ được xem, không được sửa
        public IReadOnlyCollection<ChatMessage> Messages => _messages.AsReadOnly();

        protected Conversation() { }

        public Conversation(string userId, string title)
        {
            if(string.IsNullOrWhiteSpace(userId))
            {
                throw new DomainException("UserId cannot be null or empty");
            }
            if(string.IsNullOrWhiteSpace(title))
            {
                throw new DomainException("Title cannot be null or empty");
            }
            Id = Guid.NewGuid();
            UserId = userId;
            Title = title;
            LastActive = DateTime.UtcNow;
        }

        //3. Methods
        public void AddMessage(ChatMessage message)
        {
            if(message == null)
            {
                throw new DomainException("Message cannot be null");
            }
            _messages.Add(message);
            LastActive = DateTime.UtcNow;
        }

        public void updateTitle(string newTitle)
        {
            if(string.IsNullOrWhiteSpace(newTitle))
            {
                throw new DomainException("Title cannot be null or empty");
            }
            Title = newTitle;
        }

        public void AddUserMessage(string content)
        {
            // Id của Conversation chính là this.Id
            var message = new ChatMessage(this.Id, MessageRole.User, content);
            _messages.Add(message);
            LastActive = DateTime.UtcNow;
        }

        public void AddBotMessage(string content, int tokenUsage)
        {
            var message = new ChatMessage(this.Id, MessageRole.Assistant, content, tokenUsage);
            _messages.Add(message);
            LastActive = DateTime.UtcNow;
        }

    }
}
