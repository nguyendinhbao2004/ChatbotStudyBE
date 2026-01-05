using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace ChatBotApplication.Features.Auth.Command.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("UserName is required.")
                .MaximumLength(100).WithMessage("UserName must be at least 3 characters long.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
            
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is Require.");
            RuleFor(x => x.Major)
                .NotEmpty().WithMessage("Major is Require.");
            
            RuleFor(x => x.street)
                .NotEmpty().WithMessage("Street is Require.");
            RuleFor(x => x.city)
                .NotEmpty().WithMessage("City is Require.");
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is Require.");
            RuleFor(x => x.country)
                .NotEmpty().WithMessage("Country is Require.");
            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("ZipCode is Require.");
        }
    }
}