using Domain.Common;
using Domain.Enums;
using Domain.Interface;
using Domain.Interface.Repository;
using Domain.ValueOjects;
using MediatR;

namespace ChatBotApplication.Features.Courses.Commands.UpdateCourse
{
    public class UpdateCourseHandler : IRequestHandler<UpdateCourseCommand, Result>
    {
        private readonly ICourseRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCourseHandler(ICourseRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _repo.GetByIdAsync(request.Id);
            if(course == null)
            {
                return Result<Guid>.Failure("Course not found.");
            }

            
            var newPrice = new Money(request.Price, "VND");
            try
            {
                course.UpdateInfo(request.Title, newPrice, request.Decription, request.Level);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure($"Error updating course: {ex.Message}");
            }
            _repo.Update(course);
            await _unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success("Update successful");
        }
    }
}