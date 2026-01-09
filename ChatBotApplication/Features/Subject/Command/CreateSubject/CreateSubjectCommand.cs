using Domain.Common;
using Domain.Entity;
using MediatR;

namespace ChatBotApplication.Features.Subject.Command.CreateSubject
{
    public record CreateSubjectCommand(string name, string code, List<Course> course, string? description = null) : IRequest<Result<Guid>>
    {
        
    }
}