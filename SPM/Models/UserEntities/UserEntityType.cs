using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPM.Models.UserEntities
{
    public enum UserEntityType : int
    {
        NormalUser=0,
        Company=1,
        Editor=2
    }
}
