using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.AuthorizationPolicies
{
    public class EntityTypeHandler : AuthorizationHandler<EntityTypeRequirement>
    {
        UserManager<ApplicationUser> _userManager;
            
        public EntityTypeHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EntityTypeRequirement requirement)
        {
            ApplicationUser user = await _userManager.GetUserAsync(context.User);

            if (user != null && user.EntityType == requirement.RequiredType && user.EntityId != null)
                context.Succeed(requirement);
        }
        
    }
}
