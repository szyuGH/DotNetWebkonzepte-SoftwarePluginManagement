﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models.UserEntities
{
    public class NormalUser : IUserEntity
    {
        public List<Plugin> SubscribedPlugins { get; set; }
    }
}
