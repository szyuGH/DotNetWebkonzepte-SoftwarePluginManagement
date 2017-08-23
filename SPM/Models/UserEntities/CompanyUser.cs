using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPM.Models.UserEntities
{
    public class CompanyUser : IUserEntity
    {
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Street { get; set; }

        public ICollection<Software> Softwares { get; set; }
        public ICollection<Plugin> Plugins{ get; set; }
        public ICollection<EditorUser> Editors { get; set; }
        // products and plugins and editors
    }
}
