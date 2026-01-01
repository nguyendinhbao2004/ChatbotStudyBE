using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotApplication.Dto.Course
{
    public class CreateCourseRequest
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid SubjectId { get; set; }
        public CourseLevel Level { get; set; }

    }
}
