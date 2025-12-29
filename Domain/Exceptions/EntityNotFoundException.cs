using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exception
{
    //tạo ra các Custom Exception
    public class EntityNotFoundException : System.Exception
    {
        public EntityNotFoundException() : base("The requested source was not found.")
        {

        }

        public EntityNotFoundException(string message) : base(message)
        {

        }

        public EntityNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {

        }

        // Constructor thông minh: Tự tạo message chi tiết
        // name: Tên bảng (VD: Course)
        // key: ID bị lỗi (VD: 10)
        public EntityNotFoundException(string name, object key)
            : base($"The entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
