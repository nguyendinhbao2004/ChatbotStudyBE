using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Common;
using MediatR;

namespace ChatBotApplication.Features.Courses.Commands.PublishCourse
{
    public record PublishCourseCommand(Guid Id) : IRequest<Result>
    {}
}