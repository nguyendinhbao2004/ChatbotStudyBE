using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Role() : base() { }
        public Role(string roleName, string? description = null) : base(roleName)
        {
            Description = description;
        }
    }
}
