using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Entity
{
    public class Role : IdentityRole
    {
        public Role() : base() { }
        public Role(string roleName) : base(roleName) { }

        [MaxLength(250)]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
