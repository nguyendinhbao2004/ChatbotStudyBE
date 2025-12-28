using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum EnrollmentStatus
    {
        Active = 1,     // Đang học
        Completed = 2,  // Đã hoàn thành (100%)
        Dropped = 3,    // Đã bỏ học/Hủy đăng ký
        Expired = 4     // Hết hạn truy cập
    }
}
