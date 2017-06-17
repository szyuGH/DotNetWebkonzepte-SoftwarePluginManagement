﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApplication2.Models.UserEntities;

namespace WebApplication2.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public UserEntityType EntityType { get; set; }
        public string EntityId { get; set; } 
    }
}
