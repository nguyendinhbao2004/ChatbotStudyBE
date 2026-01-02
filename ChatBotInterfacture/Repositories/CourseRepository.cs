using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatBotInterfacture.Config;
using Domain.Entity;
using Domain.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace ChatBotInterfacture.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Course> Items, int TotalCount)> GetByIdAsync(Guid id, int pageIndex, int pageSize)
        {
           var query = _context.Courses
                .Where(c => c.Id == id)
                .Include(c => c.Subject)
                .Include(c => c.Documents)
                .AsNoTracking();
            // 1. Đếm tổng số bản ghi (Quan trọng để tính TotalPages)
            var totalCount = await query.CountAsync();
            // 2. Phân trang (Skip & Take)
            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalCount);
        }

        public async Task<(IEnumerable<Course> Items, int TotalCount)> GetCoursesPagedAsync(string keyword, int pageIndex, int pageSize)
        {
            var query = _context.Courses
                .Include(c => c.Subject) // Join bảng
                .Include(c => c.Documents)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.Title.Contains(keyword));
            }

            // 1. Đếm tổng số bản ghi (Quan trọng để tính TotalPages)
            int totalCount = await query.CountAsync();

            // 2. Phân trang (Skip & Take)
            var items = await query
                .OrderByDescending(c => c.CreatedAt) // Luôn phải sort trước khi phân trang
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IEnumerable<Course> Items, int TotalCount)> GetCoursesByInstructorAsync(string instructorId, int pageIndex, int pageSize)
        {
            var query = _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                // 2. Phân trang (Skip & Take)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalCount);
        }

        public async Task<bool> IsCourseNameDuplicateAsync(string title)
        {
            return await _context.Courses.AnyAsync(c => c.Title == title);
        }
    }
}
