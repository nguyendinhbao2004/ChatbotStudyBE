using System.ComponentModel.DataAnnotations;
using ChatBotApplication.Dto.Course;
using Domain.Common;
using MediatR;

namespace ChatBotApplication.Features.Courses.Queries.GetCourseById
{
    public record GetCourseByIdQuery(Guid Id) : IRequest<Result<CourseDetailDto>>
    {
    
    }
}