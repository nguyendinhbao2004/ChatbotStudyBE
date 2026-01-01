using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Interface.Repository
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        // 1. Lấy chi tiết khóa học kèm theo thông tin Môn học (Subject) và Tài liệu (Documents)
        // Generic GetById chỉ lấy được thông tin bảng Course thôi, không lấy được bảng con.
        Task<Course> GetByIdAsync(Guid id);

        //2. Kiểm tra trùng tên khóa học (Dùng để Validate khi tạo mới)
        Task<bool> IsCourseNameDuplicateAsync(string title);

        // 3. Lấy danh sách khóa học theo Giảng viên (Instructor)
        Task<IEnumerable<Course>> GetCoursesByInstructorAsync(string instructorId);

    }
}
