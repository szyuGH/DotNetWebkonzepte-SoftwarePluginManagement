using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models.UserEntities;

namespace WebApplication2.Models
{
    public class Plugin
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public CompanyUser Company { get; set; }
        public Software RelatedSoftware { get; set; }

        public List<NormalUser> Abbonnements { get; set; }
    }
}
