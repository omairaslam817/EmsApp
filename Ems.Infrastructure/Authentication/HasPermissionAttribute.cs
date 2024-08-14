using Ems.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Infrastructure.Authentication
{
    public sealed class HasPermissionAttribute:AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission)
            :base(policy:permission.ToString()) //specifying the policy on base constuctor,this mean our permission will be treated as Policy in terms of Authrization Middle ware and we need to spcify how this policy should be handle
            //its very complex to spcify new plociy every time we add new value to permission enum,So do this very elegant way inside of ef
        {
            
        }
    }
}
