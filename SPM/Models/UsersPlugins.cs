using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SPM.Models.UserEntities;

namespace SPM.Models
{
    public class UsersPlugins
    {
        [Key]
        public string Id { get; set; }
        public NormalUser User { get; set; }
        public Plugin Plugin { get; set; }
        public Software Software { get; set; }
    }
}
