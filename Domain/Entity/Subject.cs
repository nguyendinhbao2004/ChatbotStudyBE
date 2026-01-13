using Domain.Common;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Subject : BaseEntities, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Code { get; private set; } // Vd: INT1001
        public string? Description { get; private set; }
        public bool IsActive { get; private set; } = true;

        // Một môn học có nhiều khóa học mở ra theo từng kỳ
        private readonly List<Course> _courses = new();
        public IReadOnlyCollection<Course> Courses => _courses.AsReadOnly();

        private readonly List<Document> _documents = new();
        public IReadOnlyCollection<Document> Documents => _documents.AsReadOnly();
        public Subject() { }
        public Subject(string name, string code, string? description = null)
        {
            Id = Guid.NewGuid();
            // Id đã tự sinh là Guid ở BaseEntity rồi
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));
            
            Name = name.Trim();
            Code = code.Trim().ToUpper();
            Description = description;
        }

        public void UpdateInfo(string name, string code, string? description)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = name.Trim();
            if (!string.IsNullOrWhiteSpace(code)) Code = code.Trim().ToUpper();
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
