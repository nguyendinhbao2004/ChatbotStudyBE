using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBotApplication.Features.Auth.Command.Login;
using ChatBotApplication.Features.Auth.Command.Register;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/Auth/login
        /// <summary>
        /// Dăng nhập người dùng
        /// </summary>
        /// <remarks>
        /// API này cho phép người dùng đăng nhập vào hệ thống bằng cách cung cấp tên đăng nhập và mật khẩu.
        /// <br />
        /// Nếu thông tin đăng nhập hợp lệ, hệ thống sẽ trả về một token xác thực
        /// </remarks>
        /// <param name="command">Lệnh chứa thông tin đăng nhập</param>
        /// <returns>Token xác thực nếu đăng nhập thành công</returns>
        /// <response code="200">Thành công: Trả về token xác thực</response>
        /// <response code="400">Lỗi: Thông tin đăng nhập không hợp lệ</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// Đăng ký người dùng mới
        /// </summary>
        /// <remarks>
        /// API này cho phép người dùng đăng ký tài khoản mới bằng cách cung cấp tên đăng nhập, email và mật khẩu.
        /// <br /> Nếu thông tin hợp lệ, hệ thống sẽ tạo tài khoản và trả về thông tin người dùng đã được tạo.
        /// </remarks>
        /// <param name="command">Lệnh chứa thông tin đăng ký</param>
        /// <returns>Thông tin người dùng đã được tạo nếu thành công</returns>
        /// <response code="200">Thành công: Tài khoản được tạo thành công</response>
        /// <response code="400">Lỗi: Thông tin đăng ký không hợp lệ</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
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