//using Microsoft.AspNetCore.Authorization;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Security.Claims;

//namespace Ems.Infrastructure.Authrization
//{
//    public class PermissionAuthrizationHandler : AuthorizationHandler<PermissionRequirement> //this is going to be handle as Singleton,So inject service scope
//    {
//        private readonly IServiceScopeFactory  _serviceScopeFactory;

//        public PermissionAuthrizationHandler(IServiceScopeFactory serviceScopeFactory)
//        {
//            _serviceScopeFactory = serviceScopeFactory;
//        }

//        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
//        {

//        //Second Approch:
//         //   //check if is currently authenticated user has this Permission ,if So then we can allow to continue execution.and Can Access the endpoint he want to access
//         //string? memberId =  context.User.Claims.FirstOrDefault(
//         //       x => x.Type == ClaimTypes.NameIdentifier)?.Value;
//         //   if (!Guid.TryParse(memberId, out Guid parsedMemberId))
//         //   {
//         //       return; //return not complete authrization requirement 
//         //   }//else user is authenticated and we have valid subject claim,which we are going to use if user has Permissions


//             //Two Approches If user has Permission
//             //1 Problem: AUthrization is going to be trigger every time user is trying to access your api.which mean for evry api call we are going to be database once.
//             //You can metigate this by Casching user permissions,but then you need to think about when yiu need to refresh this casche when
//             //New Permission is Added or User is Assigned to different roleAlternative
//             //approch is the ones add permission of user as Claim in JWt,So when we are handling Permission requiremnt we will just scan those claim on JWT
//             //and see if Permission from requirement is Present in the Claims,2nd approch is more performment than accessing the database on every call.
//             //but sownside is if we have longlived jwt you are not be able to going to refresh the permission of the user when New Permission is Added,
//             //SO be aware of all implemntation options,First we ll get Permssion from db in 2nd approch we will use ,How we can Add Permission as part of claims in JWT
//             //and We will chnage the implemntation to AUthrize the user base on Claims, Fetch permmsion from Db on every request,we will use IPermissionService

//            //2 Fetch from JWt claims
//             //Get Permissions from JWT,that we are sending to api,this mean we can calculate ,if user has given permission in memeory without having to goto database
//             //and perform any check there
//             //Modify JWTProvider.cs to introduce custom claims
//             //STep1: Inject Permission servcie
//             //STep2: inisde Jwt create method call permission service.get permssion for this member and then add permission as Custom Claims inside Jwt.
//             //STep 3: now we have our Permissions inside JWT use  it in PermissionAuthrizationHandler to implemnt AUthrization,
//             //comment the permission service code in above that genrate new service scope and resolve permissions service.
//             // Step4 : Now we will get Permissions from JWT,SO i am going to get rid of below permission service **
//             //create new service scope instance
//            HashSet<string> permissions = context.User.Claims //fetch Permission from Claims ,get all claims where cliam tye matches our custom claims Permissions value
//                            .Where(x => x.Type == CustomClaims.Permissions)
//                            .Select(x => x.Value)
//                            .ToHashSet(); //get only value from calim,we will end up with hashset of strings just we did in jwtOptions
//            //With Use this approch we will get rid of below logic,while where we were fetching them memeber id  from the claims and parising it into a GUID.
//            //So,Completed using permissions inside the Claims
//            //**


//            //Sendond Approch:
//            //using IServiceScope scope = _serviceScopeFactory.CreateScope();
//            //IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

//            //HashSet<string> permissions = await permissionService //retuing hashset of string containg the permissions for our member and we can use it to verify if permission contains the permission requiremnt.
//                //.GetPermissionsAsync(parsedMemberId); //Get permission for Authenticated user

//            //Two Potential Break points:
//            //1: JWT Size: Claims Permissions can potentioly grow very large size if we have big number of Permissions in app and I remember JWt token is send to API with every request.So you should condier this because you dont want to bloat the size of JWT,because its going to increase your bandwidth cost,
//            // 2: Token Lifetime: If some body gets and access token with a give set of Permissions,lets say Access Token has lifetime of 7 dyas,that mean in 7 days,JWT can be use with your api with the give set of permissions that are present in claims.
//            //even tough in reality the permissions of your member could have changes in that 7 days timeframe,at point of time the JWt was generated those information were valid,
//            //So here you potentioly think about mechanisim for revoking Acess token.when the Permission for that member have changed in the background.

//            if (permissions.Contains(requirement.Permission))
//            {
//                context.Succeed(requirement);
//            }
//            return Task.CompletedTask;
//        }
//    }
//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Ems.Infrastructure.Authrization;
using Microsoft.Extensions.DependencyInjection;

namespace Ems.Infrastructure.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // Get the user identifier (e.g., User ID or Username)
            string userId = context.User.Identity.Name;

            // Try to get the permissions from the in-memory cache
            if (!_memoryCache.TryGetValue(userId, out HashSet<string> permissions))
            {
                // If not found in in-memory cache, try to get from distributed cache (e.g., Redis)
                var cachedPermissions = await _distributedCache.GetStringAsync(userId);
                if (!string.IsNullOrEmpty(cachedPermissions))
                {
                    permissions = JsonSerializer.Deserialize<HashSet<string>>(cachedPermissions);

                    // Add to the in-memory cache for faster future access
                    _memoryCache.Set(userId, permissions, TimeSpan.FromMinutes(10)); // Adjust expiration as needed
                }
                else
                {
                    // If permissions are not in either cache, fetch from the database
                    permissions = FetchPermissionsFromDatabase(userId);

                    // Store the permissions in both caches
                    _memoryCache.Set(userId, permissions, TimeSpan.FromMinutes(10)); // Short-term in-memory cache
                    await _distributedCache.SetStringAsync(userId, JsonSerializer.Serialize(permissions), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // Long-term distributed cache
                    });
                }
            }

            // Check if the user has the required permission
            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail(); // Fail authorization if permission is not found
            }

            await Task.CompletedTask;
        }

        private HashSet<string> FetchPermissionsFromDatabase(string userId)
        {
            // Simulate fetching permissions from a database or another service
            // In a real scenario, you would inject a service like IPermissionService to fetch data from the database
            return new HashSet<string> { "Permission1", "Permission2", "Permission3" }; // Example permissions
        }
    }
}

