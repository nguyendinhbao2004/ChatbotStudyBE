using Domain.Common;
using MediatR;

namespace ChatBotApplication.Features.Courses.Commands.DeleteCourse
{
    public record DeleteCourseCommand(Guid Id) : IRequest<Result>
    {
        
    }
}