using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotInterfacture.Entity.BaseEntity
{
    public interface BaseEntities
    {
        [Key]
        public Guid Id { get; set; }  

    }
}
