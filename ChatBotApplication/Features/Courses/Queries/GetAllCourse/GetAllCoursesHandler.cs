using AutoMapper;
using ChatBotApplication.Dto.Course;
using Domain.Common;
using Domain.Interface.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotApplication.Features.Courses.Queries.GetAllCourse
{
    public class GetAllCoursesHandler : IRequestHandler<GetAllCoursesQuery, PagedResult<CourseResponse>>
    {
        private readonly ICourseRepository _repo;
        private readonly IMapper _mapper;
        public GetAllCoursesHandler(ICourseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<PagedResult<CourseResponse>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var (courses, totalRecords) = await _repo.GetCoursesPagedAsync(request.Keyword, request.PageIndex, request.PageSize);
            var courseResponses = _mapper.Map<List<CourseResponse>>(courses);
            return PagedResult<CourseResponse>.Create(courseResponses, totalRecords, request.PageIndex, request.PageSize);
        }
    }
}
