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

        public async Task<Course?> GetByIdAsync(Guid id)
        {
           return await _context.Courses
                .Include(c => c.Subject) // Eager load Subject
                .Include(c => c.Documents) // Eager load Documents
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(string instructorId)
        {
            return await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsCourseNameDuplicateAsync(string title)
        {
            return await _context.Courses.AnyAsync(c => c.Title == title);
        }
    }
}
