using ChatBotInterfacture.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Entity
{
    public class Subject
    {
        [Key]
        public int Id { get ; set ; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Code { get; set; } // Vd: INT1001

        public string Decription { get; set; }

        // Một môn học có nhiều khóa học mở ra theo từng kỳ
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
