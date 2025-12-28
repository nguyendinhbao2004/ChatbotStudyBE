using Domain.Common;
using Domain.Entity;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Document : BaseEntities, IAggregateRoot
    {


        // 1. Properties - Private Set
        public string Title { get; private set; }
        public string FilePath { get; private set; } // Đường dẫn lưu file hoặc URL Cloudinary/S3
        public long FileSize { get; private set; }   // Đổi sang long (bytes)
        public DocumentStatus Status { get; private set; } // Thay vì int/string
        public FileType FileType { get; private set; }

        // 2. Phân loại (Relationships)
        // Bắt buộc thuộc Subject
        public Guid SubjectId { get; private set; }
        public virtual Subject? Subject { get; private set; }


        // Optional: Có thể thuộc khóa học hoặc không
        public Guid? CourseId { get; private set; } // int? thay vì Guid? nếu Course dùng int
        public virtual Course? Course { get; private set; }

        // 3. Encapsulation cho Vector Chunks
        private readonly List<DocumentChunk> _chunks = new();
        public IReadOnlyCollection<DocumentChunk> DocumentChunks => _chunks.AsReadOnly();

        // 4. Constructor - Bắt buộc dữ liệu hợp lệ ngay khi tạo
        protected Document() { } // Cho EF Core

        public Document(string title, string filePath, long fileSize, FileType fileType, Guid subjectId, Guid? courseId = null)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(nameof(title));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            

            Title = title;
            FilePath = filePath;
            FileSize = fileSize;
            Id = subjectId;
            CourseId = courseId;
            Status = DocumentStatus.Pending; // Mặc định vừa tạo là Pending
            FileType = fileType;
        }

        // 5. Behavior
        public void MarkAsIndexed()
        {
            Status = DocumentStatus.Indexed;
        }
        public void AddChunk(DocumentChunk chunk)
        {
            _chunks.Add(chunk);
        }

        public void UpdateFileInfo(string newPath, long newSize)
        {
            FilePath = newPath;
            FileSize = newSize;
        }
    }
}
