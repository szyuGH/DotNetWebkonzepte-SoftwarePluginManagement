using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models.UserEntities
{
    public class NormalUser : IUserEntity
    {
        [Key]
        public string Id { get; set; }
    }
}
