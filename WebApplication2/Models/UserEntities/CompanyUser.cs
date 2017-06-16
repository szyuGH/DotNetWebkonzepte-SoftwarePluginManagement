using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models.UserEntities
{
    public class CompanyUser : IUserEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Street { get; set; }

        // products and plugins and editors
    }
}
