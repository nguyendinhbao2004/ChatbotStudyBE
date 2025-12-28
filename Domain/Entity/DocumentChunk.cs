using Domain.Common;
using Pgvector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;  
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    //DOCUMENT CHUNK(Dữ liệu Vector cho AI Search)
    public class DocumentChunk : BaseEntities
    {
        public Guid DocumentId { get; private set; } // Guid cho khớp với Document
        // Navigation optional (tránh vòng lặp nếu không cần thiết)
        // public virtual Document? Document { get; private set; } 

        public string Content { get; private set; }
        public int PageNumber { get; private set; }

        public Vector? Vector { get; private set; } // Kiểu vector chuẩn

        public DocumentChunk(Guid documentId, string content, int pageNumber, Vector? vector)
        {
            if (documentId == Guid.Empty) throw new ArgumentException("DocumentId required");
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Content required");

            DocumentId = documentId;
            Content = content;
            PageNumber = pageNumber;
            Vector = vector;
        }
    }
}
