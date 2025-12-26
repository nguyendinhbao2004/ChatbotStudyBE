using ChatBotInterfacture.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Entity
{
    public class ChatMessage : BaseEntities
    {
        public Guid Id { get ; set; }

        public Guid ConversationId { get; set; }
        [ForeignKey(nameof(ConversationId))]
        public virtual Conversation Conversation { get; set; }

        [Required]
        public string Role { get; set; } // user, assistant, system

        [Required]
        public string Content { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        public virtual ICollection<MessageCitation> Citations { get; set; }
    }
}
