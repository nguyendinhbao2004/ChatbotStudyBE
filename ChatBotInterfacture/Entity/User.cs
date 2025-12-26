using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Entity
{
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(100)]
        public string StudentId { get; set; }

        [MaxLength(100)]
        public string Major { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<Conversation> Conservations { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
