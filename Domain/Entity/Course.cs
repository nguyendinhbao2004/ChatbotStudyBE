
using Domain.Common;
using Domain.Entity;
using Domain.Enums;
using Domain.Exception;
using Domain.Exceptions;
using Domain.ValueOjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Course : BaseEntities, IAggregateRoot
    {

        public string Title { get; private set; }
        public string Description { get; private set; }
        public Money Price { get; private set; }
        public string InstructorId { get; private set; } // Link tới User

        public Guid SubjectId { get; private set; } // Bắt buộc course phải thuộc về 1 môn
        public virtual Subject? Subject { get; private set; }

        public CourseStatus Status { get; private set; }
        public CourseLevel Level { get; private set; }

        //danh sách học sinh đã đăng ký khóa học
        private readonly List<Enrollment> _enrollments = new();
        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();

        private readonly List<Document> _documents = new();
        public IReadOnlyCollection<Document> Documents => _documents.AsReadOnly();

        public Course()
        {
            
        }

        public Course(string title, Guid subjectId, string description, Money price, string instructorId, CourseLevel level)
        {
            if(string.IsNullOrWhiteSpace(title))
            {
                throw new DomainException("Title cannot be null or empty");
            }
            if(subjectId == Guid.Empty)
            {
                throw new DomainException("SubjectId cannot be empty");
            }

            Title = title;
            SubjectId = subjectId;
            Description = description;
            Price = price;
            InstructorId = instructorId;
            Level = level;
        }

        public void Publish()
        {
            // Business Rule: Phải có ít nhất 1 bài học mới được Publish
            if (_documents.Count == 0) throw new DomainException("Cannot publish empty course");
            Status = CourseStatus.Published;
        }

        public void UpdateInfo(string title, Money price, string decription, CourseLevel level)
        {
            Title = title;
            Price = price;
            Description = decription;
            level = level;
        }

        public void AddDocument(Document document)
        {
            if(document == null)
            {
                throw new DomainException("Document cannot be null");
            }
            _documents.Add(document);
            UpdatedAt = DateTime.UtcNow;
        }

        // Hàm xóa tài liệu (Soft delete hoặc xóa cứng tùy logic)
        public void RemoveDocument(Document document)
        {
            _documents.Remove(document);
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
