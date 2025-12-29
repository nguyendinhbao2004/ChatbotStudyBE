using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DomainException : System.Exception
    {
        public DomainException()
        {
            
        }

        // Constructor nhận thông báo lỗi (Cái bạn đang thiếu)
        public DomainException(string message) : base(message)
        {

        }

        // Constructor nhận thông báo lỗi và lỗi gốc (Inner Exception)
        public DomainException(string message, System.Exception innerException) : base(message, innerException)
        {
        }




    }
}
