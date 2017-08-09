using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models.UserEntities;

namespace WebApplication2.Models
{
    public class LicenseKey
    {
        [Key]
        public string Id { get; set; }
        public Software Software { get; set; }
        public NormalUser User { get; set; }
        [DisplayFormat(DataFormatString = "{0,-10}")]
        public string Key { get; set; }
    }
}
