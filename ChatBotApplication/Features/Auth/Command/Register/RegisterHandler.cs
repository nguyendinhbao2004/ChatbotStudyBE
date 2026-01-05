using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Entity;
using Domain.Interface;
using Domain.ValueOjects;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatBotApplication.Features.Auth.Command.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, Result<Guid>>
    {
        private readonly UserManager<User> _userManager;

        public RegisterHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if(existingUser !=null) return Result<Guid>.Failure("Email is already registered.");

            var address = new Address(request.street, request.city, request.State, request.country, request.ZipCode);

            var newUser = new User(request.Email, request.FullName, request.StudentId, request.Major, address);

            var Result = await _userManager.CreateAsync(newUser, request.Password);

            if(!Result.Succeeded)
            {
                return Result<Guid>.Failure(string.Join(", ", Result.Errors.Select(e => e.Description)));
            }
            await _userManager.AddToRoleAsync(newUser, "Student");
            return Result<Guid>.Success("User registered successfully.", newUser.Id);
        }
    }
}