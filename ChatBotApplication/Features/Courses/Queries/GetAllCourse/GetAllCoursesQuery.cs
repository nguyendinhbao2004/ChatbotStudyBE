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
    public class GetAllCoursesQuery : IRequest<PagedResult<CourseResponse>>
    {
        public string? Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public GetAllCoursesQuery(string? keyword, int pageIndex, int pageSize)
        {
            keyword = keyword;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
