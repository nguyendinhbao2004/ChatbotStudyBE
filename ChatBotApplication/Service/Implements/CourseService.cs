using AutoMapper;
using ChatBotApplication.Dto.Course;
using ChatBotApplication.Service.Interfaces;
using Domain.Entity;
using Domain.Exception;
using Domain.Interface;
using Domain.Interface.Repository;
using Domain.ValueOjects;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotApplication.Service.Implements
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCourseRequest> _validator;

        public CourseService(ICourseRepository repo, IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateCourseRequest> validator)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Guid> CreateCourseAsync(CreateCourseRequest request, string instructorId)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                throw new ValidationException("CreateCourseRequest validation failed: " + string.Join("; ", errors));
            }
            // 1. Map DTO sang Entity (chưa đủ dữ liệu constructor)
            // Vì Course có Constructor validate logic, ta nên new thủ công hoặc config mapper kỹ.
            // Ở đây mình new thủ công cho an toàn logic Domain.

            var priceVO = new Money(request.Price, "VND");

            var course = new Course(
                request.Title,
                request.SubjectId,
                request.Description,
                priceVO,
                instructorId
            );
            await _repo.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();
            return course.Id;
        }

        public async Task DeleteCourseAsync(Guid id)
        {
            var result = await _repo.GetByIdAsync(id);
            if(result == null)
                throw new EntityNotFoundException(nameof(Course), id);
            _repo.Delete(result);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseResponse>> GetAllCoursesAsync(string? keyword)
        {
            var course = await _repo.FindAsync(c=>
                            string.IsNullOrEmpty(keyword) || c.Title.Contains(keyword) ||
                c.Description.Contains(keyword)
            );
            return _mapper.Map<IEnumerable<CourseResponse>>(course);
        }

        public async Task<CourseResponse> GetCourseByIdAsync(Guid id)
        {
            var course = await _repo.GetByIdAsync(id);
            if(course == null)
                throw new EntityNotFoundException(nameof(Course), id);
            return _mapper.Map<CourseResponse>(course);
        }

        public async Task PublishCourseAsync(Guid id)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null)
                throw new EntityNotFoundException(nameof(Course), id);
            course.Publish();
            _repo.Update(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCourseAsync(Guid id, UpdateCourseRequest request)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null)
                throw new EntityNotFoundException(nameof(Course), id);
            var priceVO = new Money(request.Price, "VND");
            course.UpdateInfo(request.Title, priceVO, request.Description, request.Level);
            _repo.Update(course);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
