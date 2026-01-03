using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;
using MediatR;

namespace ChatBotApplication.Features.Auth.Command.Register
{
    public class RegisterCommand : IRequest<Result<Guid>>
    {
        public string Email { get; set; }
        public string FullName{ get; set; }

        public string Password { get; set; }
    }
}