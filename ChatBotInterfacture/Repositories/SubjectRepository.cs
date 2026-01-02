using ChatBotInterfacture.Config;
using Domain.Entity;
using Domain.Interface.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Subject> Items, int TotalCount)> GetByIdAsync(Guid id, int pageIndex, int pageSize)
        {
            var query = _context.Subjects
                .Where(s => s.Id == id)
                .AsNoTracking();

            // 1. Đếm tổng số bản ghi (Quan trọng để tính TotalPages)
            var totalCount = await query.CountAsync();
            // 2. Phân trang (Skip & Take)
            var items = await query
                .OrderByDescending(s => s.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalCount);
        }
    }
}
