using System.Text.Json.Serialization;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace ChatBotApplication.Features.Courses.Commands.UpdateCourse
{
    public class UpdateCourseCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string  Decription { get; set; }
        public decimal Price { get; set; }
        public Guid SubjectId { get; set; }
        public CourseLevel Level { get; set; }

        public UpdateCourseCommand(Guid id, string title, string decription, decimal price, Guid subjectId, CourseLevel level)
        {
            Id = id;
            Title = title;
            Decription = decription;
            Price = price;
            SubjectId = subjectId;
            Level = level;
        }
    }
}