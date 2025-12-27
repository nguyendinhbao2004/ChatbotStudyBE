using ChatBotInterfacture.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Entity
{
    //MESSAGE CITATION (Trích dẫn nguồn - Footnotes)
    public class MessageCitation : BaseEntities
    {
        public Guid Id { get ; set ; }
        public Guid ChatMessageId { get; set; }
        [ForeignKey(nameof(ChatMessageId))]
        public virtual ChatMessage ChatMessage { get; set; }

        // Trỏ về đoạn tài liệu nào đã được dùng để trả lời
        public Guid DocumentChunkId { get; set; }
        [ForeignKey(nameof(DocumentChunkId))]
        public virtual DocumentChunk DocumentChunk { get; set; }
        public double RelevanceScore { get; set; }
    }
}
