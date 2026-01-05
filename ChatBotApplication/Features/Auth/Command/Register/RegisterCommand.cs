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
        public string? Email { get; set; }
        public string? FullName{ get; set; }
        public string? Password { get; set; }

        public string? StudentId { get; set; }
        public string? Major { get; set; }

        public string? street { get; set; }
        public string? city { get; set; }
        public string? State { get; set; }
        public string? country { get; set; }

        public string ZipCode { get; set; }

    }
}