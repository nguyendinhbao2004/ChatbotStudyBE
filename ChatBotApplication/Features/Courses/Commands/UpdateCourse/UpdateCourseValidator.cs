using FluentValidation;

namespace ChatBotApplication.Features.Courses.Commands.UpdateCourse
{
    public class UpdateCourseValidator : AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Course ID is required.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Course name is required.");
            RuleFor(x=> x.Decription).NotEmpty().WithMessage("Course description is required.");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be non-negative.");
            RuleFor(x => x.Level).IsInEnum().WithMessage("Invalid course level.");
            RuleFor(x => x.SubjectId).NotEmpty().WithMessage("Subject ID is required.");
        }
    }
}