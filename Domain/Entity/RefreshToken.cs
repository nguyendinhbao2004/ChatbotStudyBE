using Domain.Common;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    
    //Mối quan hệ giữa User và Session (Phiên đăng nhập) là 1 - N (Một - Nhiều).
    //Hỗ trợ đa thiết bị (Multi-Device Support)
    //Tính năng "Đăng xuất từ xa" (Revoke Specific Device
    public class RefreshToken : BaseEntities
    {
        public Guid UserId { get; private set; } // IdentityUser dùng String ID
        public string Token { get; private set; }
        public string JwtId { get; private set; } // JTI
        public bool IsUsed { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime Expires { get; private set; }
        public DateTime AddedDate { get; private set; } = DateTime.UtcNow;


        protected RefreshToken()
        {
            
        }
        // Constructor
        public RefreshToken(Guid userId, string token, string jwtId, DateTime expires)
        {
            if (userId == Guid.Empty) throw new DomainException("UserId cannot be empty.");
            if (string.IsNullOrWhiteSpace(token)) throw new DomainException("Token cannot be empty.");

            UserId = userId;
            Token = token;
            JwtId = jwtId;
            Expires = expires;
            
            // Giá trị mặc định
            IsUsed = false;
            IsRevoked = false;
            AddedDate = DateTime.UtcNow;
        }

        public void Revoke()
        {
            IsRevoked = true;
        }

        public void Use()
        {
            IsUsed = true;
        }

    }
}
