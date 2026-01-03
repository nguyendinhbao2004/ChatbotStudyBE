using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Entity;
using Domain.Interface;
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

            var newUser = new User(request.Email, request.FullName);

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