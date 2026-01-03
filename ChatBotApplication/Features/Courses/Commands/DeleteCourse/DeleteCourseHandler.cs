using Domain.Common;
using Domain.Interface;
using Domain.Interface.Repository;
using MediatR;

namespace ChatBotApplication.Features.Courses.Commands.DeleteCourse
{
    public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, Result>
    {
        private readonly ICourseRepository _repo;
        private readonly IUnitOfWork unitOfWork;

        public DeleteCourseHandler(ICourseRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _repo.GetByIdAsync(request.Id);
            if (course == null)
            {
                return Result<Guid>.Failure("Course not found.");
            }

            course.IsDeleted = true;
            course.UpdatedAt = DateTime.UtcNow;
            _repo.Update(course);
            await unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(request.Id);
        }
    }
}