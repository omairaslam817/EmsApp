﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Infrastructure.Authrization
{
    public class PermissionRequirement : IAuthorizationRequirement

    {
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
        public string Permission { get;}
    }
}
