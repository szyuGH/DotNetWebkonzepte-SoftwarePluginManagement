using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPM.Models.UserEntities
{
    public class EditorUser : IUserEntity
    {
        public CompanyUser Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
