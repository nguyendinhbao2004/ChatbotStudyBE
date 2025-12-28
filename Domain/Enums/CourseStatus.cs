using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum CourseStatus
    {
        Draft = 1,      // Nháp (chỉ GV thấy)
        Published = 2,  // Công khai (Học viên thấy)
        Archived = 3,   // Lưu trữ (Không cho đăng ký mới nhưng vẫn xem được)
        Deleted = 4     // Đã xóa (Soft delete logic nghiệp vụ)
    }
}
