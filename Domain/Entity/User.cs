using Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class User : IdentityUser, IAggregateRoot
    {
        public string FullName { get; private set; }
        public string? StudentId { get; private set; } // Mã sinh viên
        public string? Major { get; private set; }

        // Encapsulation
        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        private readonly List<Conversation> _conversations = new();
        public IReadOnlyCollection<Conversation> Conversations => _conversations.AsReadOnly();

        private readonly List<Enrollment> _enrollments = new();
        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();

        // Constructor mặc định cho EF Core
        public User() { }

        // Behavior Methods
        public void UpdateInfo(string fullName, string? studentId, string? major)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentNullException(nameof(fullName));
            FullName = fullName;
            StudentId = studentId;
            Major = major;
        }

        public void AddRefreshToken(string token, string jwtId, DateTime expires)
        {
            // Id là User.Id (kiểu string của Identity)
            var rt = new RefreshToken(this.Id, token, jwtId, expires);
            _refreshTokens.Add(rt);
        }
    }
}
