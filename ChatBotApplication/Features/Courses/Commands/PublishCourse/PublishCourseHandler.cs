using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Interface;
using Domain.Interface.Repository;
using MediatR;

namespace ChatBotApplication.Features.Courses.Commands.PublishCourse
{
    public class PublishCourseHandler : IRequestHandler<PublishCourseCommand, Result>
    {
        private readonly ICourseRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public PublishCourseHandler(ICourseRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(PublishCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _repo.GetByIdAsync(request.Id);
            if (course == null)
            {
                return Result.Failure("Course not found");
            }

            course.Publish();
            _repo.Update(course);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Course published successfully");
        }
    }
}