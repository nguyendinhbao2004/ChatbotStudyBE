using ChatBotApplication.Dto.Course;
using ChatBotApplication.Features.Courses.Commands.CreateCourse;
using ChatBotApplication.Features.Courses.Queries.GetAllCourse;
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

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Course>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCourses(
            [FromQuery] string? keyword, 
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCoursesQuery(keyword, pageIndex, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseCommand command)
        {
            command.InstructorId = "user-123"; // Giả sử lấy từ token
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
