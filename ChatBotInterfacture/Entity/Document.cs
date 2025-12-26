using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Entity
{
    public class Document 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string FilePath { get; set; } //.pdf, .docx
        public string FileSize { get; set; }

        public bool IsProcessed { get; set; } //đã tách vector chưa 
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Phân loại tài liệu:
        // 1. Thuộc Subject nào? (Bắt buộc để phân loại)
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public virtual Subject Subject { get; set; }

        // 2. (Optional) Có thuộc riêng khóa học nào không? 
        // Nếu null -> Tài liệu chung. Nếu có -> Tài liệu riêng của lớp.
        public Guid? CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        public virtual ICollection<DocumentChunk> DocumentChunks { get; set; }
    }
}
