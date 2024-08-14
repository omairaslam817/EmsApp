using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Ems.Infrastructure.Authrization
{
    public class PermissionAuthrizationHandler : AuthorizationHandler<PermissionRequirement> //this is going to be handle as Singleton,So inject service scope
    {
        private readonly IServiceScopeFactory  _serviceScopeFactory;

        public PermissionAuthrizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //check if is currently authenticated user has this Permission ,if So then we can allow to continue execution.and Can Access the endpoint he want to access
         string? memberId =  context.User.Claims.FirstOrDefault(
                x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(memberId, out Guid parsedMemberId))
            {
                return; //return not complete authrization requirement 
            }//else user is authenticated and we have valid subject claim,which we are going to use if user has Permissions
              //Two Approches If user has Permission
              //1 Problem: AUthrization is going to be trigger every time user is trying to access your api.which mean for evry api call we are going to be database once.
              //You can metigate this by Casching user permissions,but then you need to think about when yiu need to refresh this casche when New Permission is Added or User is Assigned to different roleAlternative
              //approch is the ones add permission of user as Claim in JWt,So when we are handling Permission requiremnt we will just scan those claim on JWT
              //and see if Permission from requirement is Present in the Claims,2nd approch is more performment than accessing the database on every call.
              //but sownside is if we have longlived jwt you are not be able to going to refresh the permission of the user when New Permission is Added,
              //SO be aware of all implemntation options,First we ll get Permssion from db in 2nd approch we will use ,How we can Add Permission as part of claims in JWT
              //and We will chnage the implemntation to AUthrize the user base on Claims, Fetch permmsion from Db on every request,we will use IPermissionService
              //2 Fetch from JWt claims
             
            //create new service scope instance
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

            HashSet<string> permissions = await permissionService //retuing hashset of string containg the permissions for our member and we can use it to verify if permission contains the permission requiremnt.
                .GetPermissionsAsync(parsedMemberId);
            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
