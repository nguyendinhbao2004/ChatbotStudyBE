using ChatBotInterfacture.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Entity
{
    public class RefreshToken : BaseEntities
    {
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public string Token { get; set; }
        public string JwtId { get; set; }

        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }

        public DateTime Expires { get; set; }
        public DateTime AddedDate { get; set; }

    }
}
