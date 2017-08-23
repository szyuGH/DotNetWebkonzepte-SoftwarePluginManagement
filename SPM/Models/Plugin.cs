using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SPM.Models.UserEntities;

namespace SPM.Models
{
    public class Plugin
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public byte[] Data { get; set; }

        public CompanyUser Company { get; set; }
        [Display(Name="Software")]
        public Software RelatedSoftware { get; set; }


        public bool CanEdit(IUserEntity entity)
        {
            return (entity is EditorUser && (entity as EditorUser).Company == Company)
                || (entity is CompanyUser && Company == entity);
        }
    }
}
