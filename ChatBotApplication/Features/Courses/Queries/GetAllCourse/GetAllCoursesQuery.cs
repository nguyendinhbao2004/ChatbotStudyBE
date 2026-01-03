using ChatBotApplication.Dto.Course;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotApplication.Features.Courses.Queries.GetAllCourse
{
    public record GetAllCoursesQuery(string? Keyword, int PageIndex, int PageSize) : IRequest<PagedResult<CourseResponse>>
    {

    }
}
//giống với 
// public class CourseQuery
// {
//     public string Keyword { get; }
//     public int PageIndex { get; }

//     public CourseQuery(string keyword, int pageIndex)
//     {
//         Keyword = keyword;
//         PageIndex = pageIndex;
//     }
// }