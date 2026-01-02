using ChatBotApplication.Dto.Course;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotApplication.Features.Courses.Commands.CreateCourse
{
    public class CreateCourseValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseValidator()
        {
            // 1. Kiểm tra cấu trúc (Format)
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required");

            // 2. Kiểm tra giá trị cơ bản
            RuleFor(x => x.SubjectId)
                .NotEmpty().WithMessage("SubjectId is required");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero");

            // 3. Kiểm tra Enum
            RuleFor(x => x.Level)
                .IsInEnum().WithMessage("Level is not valid");
        }
    }
}
