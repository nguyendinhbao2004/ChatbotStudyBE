using System.Data;
using FluentValidation;

namespace ChatBotApplication.Features.Subject.Command.CreateSubject
{
    public class CreateSubjectValidator : AbstractValidator<CreateSubjectCommand>
    {
        public CreateSubjectValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Name of subject is require")
                .MaximumLength(100).WithMessage("Title must not exceed 200 characters");
            
            RuleFor(x => x.code)
                .NotEmpty().WithMessage("Code of subject is require")
                .MaximumLength(20).WithMessage("Code must not exceed 20 characters");
            
            RuleFor(x => x.course)
                .NotNull().WithMessage("Course is require");

            RuleFor(x => x.description)
                .NotEmpty().WithMessage("Decription is require")
                .MaximumLength(200).WithMessage("Decription must not exceed 200 characters");

        }
    }
}