using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum DocumentStatus
    {
        Pending = 1,    // Vừa upload, chưa làm gì
        Processing = 2, // Đang cắt chunk và tạo vector (Background Job đang chạy)
        Indexed = 3,    // Đã xong, sẵn sàng để search
        Failed = 4      // Lỗi (file hỏng, lỗi API AI...)
    }
}
