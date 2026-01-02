using Domain.Common;
using Domain.Entity;
using Domain.Interface;
using Domain.Interface.Repository;
using Domain.ValueOjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotApplication.Features.Courses.Commands.CreateCourse
{
    public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, Result<Guid>>
    {
        private readonly ICourseRepository _repo;
        private readonly ISubjectRepository _subjectRepo;
        private readonly IUnitOfWork _unitOfWork;
        public CreateCourseHandler(ICourseRepository repo, IUnitOfWork unitOfWork, ISubjectRepository subjectRepository)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _subjectRepo = subjectRepository;
        }
        public async Task<Result<Guid>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var priceVO = new Money(request.Price, "VND");

            var subjectsId = await _subjectRepo.GetByIdAsync(request.SubjectId);
            if (subjectsId == null)
            {
                return Result<Guid>.Failure("Subject not found");
            }

            var course = new Course(
                request.Title,
                request.SubjectId,
                request.Description,
                priceVO,
                request.InstructorId,
                request.Level
            );

            await _repo.AddAsync(course);
            await _unitOfWork.SaveChangesAsync(cancellationToken);// Truyền token để hủy nếu client ngắt kết nối

            return Result<Guid>.Success("Course created successfully", course.Id);
        }
    }
}
