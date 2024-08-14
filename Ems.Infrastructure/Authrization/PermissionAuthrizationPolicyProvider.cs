using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Infrastructure.Authrization
{
    public class PermissionAuthrizationPolicyProvider 
        : DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthrizationPolicyProvider(IOptions<AuthorizationOptions> options) 
            : base(options)
        {
        }
        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {//use this method to see of policy with this policyname here already exist,policyName here will come from HaspermissionAttribute 
           AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName); //this vale can be null ,and this can be when ploicy is not defined in that case create new policy,first hanle opposite case
            if (policy is not null)
            {
                return policy;
            }
            return new AuthorizationPolicyBuilder() //policy doest not yet exist So we need to create it,Pass polcy name as permission name for our requiremnt
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();  ///use build for handle authrization policy and next time this method is going to called our autrization policy already going to exist.and we dont need to create new one.This how automatic define missing policies for when you define new Permission enum values and you dont haev to think about it
            //config permission authrization policy provider as singleton So lets do that in Prohram.cs
        }
    }
}
