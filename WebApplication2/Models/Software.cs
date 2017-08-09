using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models.UserEntities;

namespace WebApplication2.Models
{
    public class Software
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public CompanyUser Company { get; set; }

        public ICollection<LicenseKey> LicenseKeys { get; set; }
        public ICollection<Plugin> Plugins { get; set; }

        public bool CanEdit(IUserEntity entity)
        {
            return (entity is EditorUser && (entity as EditorUser).Company == Company)
                || (entity is CompanyUser && Company == entity);
        }
    }
}
