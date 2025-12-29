using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    //đảm bảo tính toàn vẹn dữ liệu (Data Consistency) khi thao tác trên nhiều bảng cùng lúc
    //khả năng lưu tất cả thay đổi xuống Database cùng một lúc.
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
