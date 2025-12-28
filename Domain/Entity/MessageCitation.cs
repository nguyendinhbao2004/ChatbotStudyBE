using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    //MESSAGE CITATION (Trích dẫn nguồn - Footnotes)
    public class MessageCitation : BaseEntities
    {
        public Guid ChatMessageId { get; private set; }
        public virtual ChatMessage? ChatMessage { get; private set; }

        public Guid DocumentChunkId { get; private set; }
        public virtual DocumentChunk? DocumentChunk { get; private set; }

        public double RelevanceScore { get; private set; } // Điểm tương đồng (Similarity)

        public MessageCitation(Guid chatMessageId, Guid documentChunkId, double relevanceScore)
        {
            ChatMessageId = chatMessageId;
            DocumentChunkId = documentChunkId;
            RelevanceScore = relevanceScore;
        }
    }
}
