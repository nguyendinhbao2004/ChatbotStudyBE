using ChatBotInterfacture.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Entity
{
    public class Course : BaseEntities
    {
        public Guid Id { get ; set ; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Semester { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public virtual Subject Subject { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
