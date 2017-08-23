using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SPM.Models.UserEntities;

namespace SPM.Models
{
    public class LicenseKey
    {
        [Key]
        public string Id { get; set; }
        public Software Software { get; set; }
        public NormalUser User { get; set; }
        [Required]
        public string Key { get; set; }

        public string GetShortKey(int length)
        {
            if (Key.Length > length)
            {
                return Key.Substring(0, length) + "...";
            } else
            {
                return Key;
            }
        }

        public string GetFullUsername()
        {
            if (User == null)
            {
                return "No assigned User";
            } else
            {
                return User.GetFullName();
            }
        }
    }
}
