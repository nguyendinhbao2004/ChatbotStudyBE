using ChatBotApplication.Dto.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotApplication.Service.Interfaces
{
    public interface ICourseService
    {
        // 1. Lấy danh sách (có tìm kiếm)
        Task<IEnumerable<CourseResponse>> GetAllCoursesAsync(string? keyword);

        // 2. Lấy chi tiết khóa học
        Task<CourseResponse> GetCourseByIdAsync(Guid id);

        // 3. Tạo mới khóa học
        Task<Guid> CreateCourseAsync(CreateCourseRequest request, string instructorId);

        // 4. Cập nhật thông tin khóa học
        Task UpdateCourseAsync(Guid id, UpdateCourseRequest request);

        // 5. Xóa khóa học
        Task DeleteCourseAsync(Guid id);

        //6. Nghiệp vụ riêng: Công khai khóa học
        Task PublishCourseAsync(Guid id);
    }
}
