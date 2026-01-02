using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Repository
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        Task<(IEnumerable<Subject> Items, int TotalCount)> GetByIdAsync(Guid id, int pageIndex, int pageSize);
    }
}
