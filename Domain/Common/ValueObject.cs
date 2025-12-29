using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        // Hàm này bắt buộc các lớp con phải khai báo xem so sánh dựa trên thuộc tính nào
        protected abstract IEnumerable<object> GetEqualityComponents();
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }
        // Overload toán tử == và != để so sánh tiện hơn
        public static bool operator ==(ValueObject left, ValueObject right)
        {
            return EqualOperator(left, right);
        }
        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return NotEqualOperator(left, right);
        }
    }
}
