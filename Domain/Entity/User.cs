using Domain.Common;
using Domain.ValueOjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class User : IdentityUser<Guid>, IAggregateRoot
    {
        public string FullName { get; private set; }
        public string? StudentId { get; private set; } // Mã sinh viên
        public string? Major { get; private set; }

        public Address? Address { get; private set; }

        // Encapsulation
        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        private readonly List<Conversation> _conversations = new();
        public IReadOnlyCollection<Conversation> Conversations => _conversations.AsReadOnly();

        private readonly List<Enrollment> _enrollments = new();
        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();

        // Constructor mặc định cho EF Core
        public User() { }

        public User(string email, string fullName, string studentId, string major, Address address) : base()
        {
            Id = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentNullException(nameof(fullName));
            if(address == null) throw new ArgumentNullException(nameof(address));
            if (string.IsNullOrWhiteSpace(studentId)) throw new ArgumentNullException(nameof(studentId));
            if (string.IsNullOrWhiteSpace(major)) throw new ArgumentNullException(nameof(major));
            Id = Guid.NewGuid();
            UserName = email;
            Email = email;
            FullName = fullName;
            StudentId = studentId;
            Major = major;
            Address = address;
            SecurityStamp = Guid.NewGuid().ToString();
        }

        // Behavior Methods
        public void UpdateInfo(string fullName, string? studentId, string? major)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentNullException(nameof(fullName));
            FullName = fullName;
            StudentId = studentId;
            Major = major;
        }

        public void AddRefreshToken(string token, string jwtId, int expiryDays = 30)
        {
            // Id là User.Id (kiểu string của Identity)
            // User tự tạo ra RefreshToken cho chính mình
            var refreshToken = new RefreshToken(this.Id, token, jwtId, DateTime.UtcNow.AddDays(expiryDays));
            _refreshTokens.Add(refreshToken);
        }

        public void UpdateAddress(string street, string city, string country)
        {
            // Khi thay đổi địa chỉ, ta TẠO MỚI (Immutability)
            Address = new Address(street, city, "", country, "");
        }
    }
}
