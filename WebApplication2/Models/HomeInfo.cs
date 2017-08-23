using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class HomeInfo
    {
        [Display(Name = "Registrated Companies")]
        public int CompanyCount { get; set; }
        [Display(Name = "Registrated Users")]
        public int UserCount { get; set; }
        [Display(Name = "Registrated Softwares")]
        public int SoftwareCount { get; set; }
        [Display(Name = "Registrated Plugins")]
        public int PluginCount { get; set; }
    }
}
