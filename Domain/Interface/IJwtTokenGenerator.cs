using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Interface
{
    public interface IJwtTokenGenerator
    {
        // Hàm sinh Access Token (kèm Roles)
        string GenerateToken(User user, IList<string> roles);

        // Hàm sinh Refresh Token
        string GenerateRefreshToken();
    }
}