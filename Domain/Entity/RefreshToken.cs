using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class RefreshToken : BaseEntities
    {
        public string UserId { get; private set; } // IdentityUser dùng String ID
        public string Token { get; private set; }
        public string JwtId { get; private set; } // JTI
        public bool IsUsed { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime Expires { get; private set; }
        public DateTime AddedDate { get; private set; } = DateTime.UtcNow;

        // Constructor
        public RefreshToken(string userId, string token, string jwtId, DateTime expires)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("UserId required");

            UserId = userId;
            Token = token;
            JwtId = jwtId;
            Expires = expires;
            IsUsed = false;
            IsRevoked = false;
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
