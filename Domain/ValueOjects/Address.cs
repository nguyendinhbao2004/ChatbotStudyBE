using Domain.Common;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueOjects
{
    public class Address : ValueObject
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }
        public Address(string street, string city, string state, string country, string zipCode)
        {
            if (string.IsNullOrWhiteSpace(street)) throw new DomainException("Street required");
            if (string.IsNullOrWhiteSpace(city)) throw new DomainException("City required");
            if (string.IsNullOrWhiteSpace(country)) throw new DomainException("Country required");
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // yield return: Trả về lần lượt từng phần tử (Lazy evaluation).
            // Nếu Street khác nhau -> Dừng lại, kết luận khác nhau ngay (tối ưu hiệu năng).
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return ZipCode;
        }

        public static Address Create(string street, string city, string country)
        {
            return new Address(street, city, "", country, "");
        }
    }
}
