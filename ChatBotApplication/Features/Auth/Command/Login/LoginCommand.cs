using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBotApplication.Dto.Auth;
using Domain.Common;
using MediatR;

namespace ChatBotApplication.Features.Auth.Command.Login
{
    public class LoginCommand : IRequest<Result<AuthResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}