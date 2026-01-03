using AutoMapper;
using ChatBotApplication.Dto.Course;
using Domain.Common;
using Domain.Interface.Repository;
using MediatR;

namespace ChatBotApplication.Features.Courses.Queries.GetCourseById
{
    public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, Result<CourseDetailDto>>
    {
        private readonly ICourseRepository _repo;
        private readonly IMapper _mapper;
        public GetCourseByIdHandler(ICourseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<Result<CourseDetailDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await _repo.GetByIdAsync(request.Id);
            if (course == null)
            {
                return Result<CourseDetailDto>.Failure($"Không tìm thấy khóa học với Id: {request.Id}");
            }
            var courseDetailDto = _mapper.Map<CourseDetailDto>(course);
            return Result<CourseDetailDto>.Success(courseDetailDto);
        }
    }
}