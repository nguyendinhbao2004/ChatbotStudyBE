using ChatBotApplication.Dto.Course;
using ChatBotApplication.Features.Courses.Commands.CreateCourse;
using ChatBotApplication.Features.Courses.Commands.DeleteCourse;
using ChatBotApplication.Features.Courses.Commands.PublishCourse;
using ChatBotApplication.Features.Courses.Commands.UpdateCourse;
using ChatBotApplication.Features.Courses.Queries.GetAllCourse;
using ChatBotApplication.Features.Courses.Queries.GetCourseById;
using Domain.Common;
using Domain.Entity;
using Domain.Interface.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh sách tất cả khóa học với phân trang và tìm kiếm
        /// </summary>
        /// <remarks>
        /// API này trả về danh sách các khóa học dựa trên từ khóa tìm kiếm (nếu có) và hỗ trợ phân trang.
        /// <br />
        /// **Yêu cầu:** User phải đăng nhập.
        /// </remarks>
        /// <param name="keyword">Từ khóa tìm kiếm trong tên khóa học (tùy chọn)</param>
        /// <param name="pageIndex">Chỉ số trang (bắt đầu từ 1)</param>
        /// <param name="pageSize">Số lượng bản ghi trên mỗi trang</param>
        /// <returns>Danh sách các khóa học dưới dạng phân trang</returns>
        /// <response code="200">Thành công: Trả về danh sách khóa học</response>
        /// <response code="404">Lỗi: Không tìm thấy khóa học</response>
        /// <response code="401">Lỗi: Chưa đăng nhập (Token không hợp lệ)</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Course>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllCourses(
            [FromQuery] string? keyword, 
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCoursesQuery(keyword, pageIndex, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        /// <summary>
        /// Tạo một khóa học mới
        /// </summary>
        /// <remarks>
        /// API này cho phép tạo mới một khóa học với các thông tin như tên khóa học, giá, môn học và giảng viên.
        /// <br />
        /// **Yêu cầu:** User phải đăng nhập.
        /// </remarks>
        /// <param name="command">Đối tượng chứa thông tin khóa học cần tạo</param>
        /// <returns>Đối tượng khóa học đã được tạo</returns>
        /// <response code="201">Thành công: Khóa học được tạo thành công</response>
        /// <response code="400">Lỗi: Dữ liệu đầu vào không hợp lệ</response>
        /// <response code="401">Lỗi: Chưa đăng nhập (Token không hợp lệ)</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseCommand command)
        {
            command.InstructorId = "user-123"; // Giả sử lấy từ token
            var result = await _mediator.Send(command);
            if(!result.Succeeded)
            {
                // 3. Kiểm tra logic để biết đây là lỗi 404 (Không tìm thấy)
        // (Cách đơn giản nhất là check message hoặc data null)
                if(result.Message.Contains("not found"))
                {
                    // 👇 QUAN TRỌNG: Truyền 'result' vào đây!
            // Nó sẽ lấy toàn bộ Message và Errors từ Handler để hiển thị ra JSON
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Lấy chi tiết một khóa học theo ID
        /// </summary>
        /// <remarks>
        /// API này trả về thông tin đầy đủ của khóa học bao gồm: Tên, giá, môn học và giảng viên.
        /// <br />
        /// **Yêu cầu:** User phải đăng nhập.
        /// </remarks>
        /// <param name="id">Mã định danh (GUID) của khóa học</param>
        /// <returns>Đối tượng chi tiết khóa học</returns>
        /// <response code="200">Thành công: Trả về dữ liệu khóa học</response>
        /// <response code="404">Lỗi: Không tìm thấy khóa học với ID này</response>
        /// <response code="401">Lỗi: Chưa đăng nhập (Token không hợp lệ)</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CourseDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetCourseById([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid course ID");
            }
            var query = new GetCourseByIdQuery(id);
            var result = await _mediator.Send(query);
            if (!result.Succeeded)
            {
                if(result.Message.Contains("not found"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Cập nhật thông tin một khóa học
        /// </summary>
        /// <remarks>
        /// API này cho phép cập nhật thông tin của một khóa học dựa trên ID.
        /// <br /> **Yêu cầu:** User phải đăng nhập.
        /// </remarks>
        /// <param name="id">Mã định danh (GUID) của khóa học</param>
        /// <param name="command">Đối tượng chứa thông tin khóa học cần cập nhật</param>
        /// <returns>Đối tượng khóa học đã được cập nhật</returns>
        /// <response code="200">Thành công: Khóa học được cập nhật thành công</response>
        /// <response code="400">Lỗi: Dữ liệu đầu vào không hợp lệ</response>
        /// <response code="404">Lỗi: Không tìm thấy khóa học với ID này</response>
        /// <response code="401">Lỗi: Chưa đăng nhập (Token không hợp lệ)</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateCourse([FromRoute] Guid id, [FromBody] UpdateCourseCommand command)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid course ID");
            }
            command.Id = id;
            var result = await _mediator.Send(command);
            if (!result.Succeeded)
            {
                if(result.Message.Contains("Update failed"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Xóa một khóa học theo ID
        /// </summary>
        /// <remarks>
        /// API này cho phép xóa một khóa học dựa trên ID.
        /// <br /> **Yêu cầu:** User phải đăng nhập.
        /// </remarks>
        /// <param name="id">Mã định danh (GUID) của khóa học</param>
        /// <returns>Xóa thành công khóa học</returns>
        /// <response code="200">Thành công: Khóa học được xóa thành công</response>
        /// <response code="404">Lỗi: Không tìm thấy khóa học với ID này</response>
        /// <response code="401">Lỗi: Chưa đăng nhập (Token không hợp lệ)</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteCourse([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid course ID");
            }
            var command = new DeleteCourseCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        /// <summary>
        /// Publish một khóa học theo ID
        /// </summary>
        /// <remarks>
        /// API này cho phép Publish một khóa học dựa trên ID.  
        /// <br /> **Yêu cầu:** User phải đăng nhập.
        /// </remarks>
        /// <param name="id">Nhập Id của Course</param>
        /// <returns>Publish Course thành công</returns>
        /// <response code="200">Thành công: Khóa học được Publish thành công</response>
        /// <response code="404">Lỗi: Không tìm thấy khóa học với ID này</response>
        /// <response code="400">Lỗi: Không thể Publish khóa học rỗng</response>
        /// <response code="401">Lỗi: Chưa đăng nhập (Token không hợp lệ)</response>
        [Authorize]
        [HttpPut("publish/{id}")]
        public async Task<IActionResult> PublishCourse([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid course ID");
            }
            var command = new PublishCourseCommand(id);
            var result = await _mediator.Send(command);
            if (!result.Succeeded)
            {
                if(result.Message.Contains("not found"))
                {
                    return NotFound(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}