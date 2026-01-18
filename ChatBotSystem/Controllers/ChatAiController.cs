using ChatBotApplication.Features.Chat.Command;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatAiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatAiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Ai gửi và nhận tin nhắn trong cuộc trò chuyện
        /// </summary>
        /// <remarks>
        /// API này cho phép người dùng gửi tin nhắn đến AI trong một cuộc trò chuyện cụ thể.
        /// <br /> AI sẽ xử lý tin nhắn và trả về phản hồi dựa trên nội dung đã gửi.
        /// </remarks>
        /// <param name="command"></param>
        /// <returns>
        /// Phản hồi từ AI dưới dạng chuỗi nếu thành công
        /// </returns> 
        /// <response code="200">Thành công: Trả về phản hồi từ AI</response>
        /// <response code="400">Lỗi: Nội dung tin nhắn không hợp lệ</response>
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("SendAiMessage")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendAiMessage([FromBody] SendMessageCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}