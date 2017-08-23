using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models.UserEntities
{
    public class NormalUser : IUserEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<LicenseKey> LicenseKeys { get; set; }
        public ICollection<UsersPlugins> Plugins { get; set; }


        public string GetFullName()
        {
            return string.Format("{0}, {1}", LastName, FirstName);
        }

        public bool SubscribedPlugin(string pluginId)
        {
            return Plugins.Any(p => p.User == this && p.Plugin.Id == pluginId);
        }
    }
}
