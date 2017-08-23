using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SPM.Models.UserEntities;

namespace SPM.AuthorizationPolicies
{
    public class EntityTypeRequirement : IAuthorizationRequirement
    {
        public UserEntityType RequiredType { get; set; }

        public EntityTypeRequirement(UserEntityType type)
        {
            RequiredType = type;
        }
    }
}
