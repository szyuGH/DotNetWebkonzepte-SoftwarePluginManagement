using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models.UserEntities
{
    public class EditorUser : IUserEntity
    {
        public string Id { get; set; }
        public CompanyUser Company { get; set; }
    }
}
