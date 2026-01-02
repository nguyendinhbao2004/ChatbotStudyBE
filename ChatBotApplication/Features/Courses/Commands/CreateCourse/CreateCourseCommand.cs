using Domain.Common;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatBotApplication.Features.Courses.Commands.CreateCourse
{
    public class CreateCourseCommand : IRequest<Result<Guid>>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid SubjectId { get; set; }
        public CourseLevel Level { get; set; }
        [JsonIgnore]
        public string? InstructorId { get; set; } // Sẽ được gán từ Controller
    }
}
