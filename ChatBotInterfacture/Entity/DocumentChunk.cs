using ChatBotInterfacture.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Entity
{
    //DOCUMENT CHUNK(Dữ liệu Vector cho AI Search)
    public class DocumentChunk : BaseEntities
    {
        public Guid Id { get; set; }

        public int DocumentId { get; set; }
        [ForeignKey(nameof(DocumentId))]
        public virtual Document Document { get; set; }

        [Required]
        public string Content { get; set; }
        public int PageNumber { get; set; }

        // VECTOR EMBEDDING
        // Nếu dùng PostgreSQL + pgvector: Cột này sẽ map vào kiểu 'vector(1536)'
        // Ở đây khai báo float[] để code C# hiểu.
        public float[] Vector { get; set; }
    }
}
