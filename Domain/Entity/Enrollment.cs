using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Enrollment : BaseEntities
    {
        public string UserId { get; private set; } // FK String (Identity)
        public virtual User? Student { get; private set; }

        public Guid CourseId { get; private set; } // FK Guid (Course)
        public virtual Course? Course { get; private set; }

        public DateTime EnrollAt { get; private set; } = DateTime.UtcNow;
        public float Progress { get; private set; } // 0.0 - 100.0

        protected Enrollment() { }

        public Enrollment(string userId, Guid courseId)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("User required");
            if (courseId == Guid.Empty) throw new ArgumentException("Course required");

            UserId = userId;
            CourseId = courseId;
            Progress = 0;
        }

        public void UpdateProgress(float newProgress)
        {
            if (newProgress < 0 || newProgress > 100)
                throw new ArgumentException("Progress must be between 0 and 100");

            Progress = newProgress;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
