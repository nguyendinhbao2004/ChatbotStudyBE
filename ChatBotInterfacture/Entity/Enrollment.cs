using ChatBotInterfacture.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Entity
{
    public class Enrollment : BaseEntities
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User Student { get; set; }

        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        public DateTime EnrollAt { get; set; } = DateTime.UtcNow;

        public float Progress { get; set; } //Tiến độ 0-100%
    }
}
