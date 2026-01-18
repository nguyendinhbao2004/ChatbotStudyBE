using Domain.Common;
using MediatR;

namespace ChatBotApplication.Features.Chat.Command
{
    public record SendMessageCommand(Guid ConversationId, string content, string UserId) : IRequest<Result<string>>
    {
        
    }
}