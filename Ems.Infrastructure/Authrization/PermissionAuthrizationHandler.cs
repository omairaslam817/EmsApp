using Ems.Infrastructure.Authrization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ems.Infrastructure.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMemoryCache _memoryCache;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory, IMemoryCache memoryCache)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _memoryCache = memoryCache;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // Get the user identifier (e.g., User ID or Username)
            string userId = context.User.Identity.Name;

            // Try to get the permissions from the in-memory cache
            if (!_memoryCache.TryGetValue(userId, out HashSet<string> permissions))
            {
                // If permissions are not found in the cache, fetch from the database
                permissions = FetchPermissionsFromDatabase(userId);

                // Store the permissions in the in-memory cache for future requests
                _memoryCache.Set(userId, permissions, TimeSpan.FromMinutes(10)); // Adjust expiration as needed
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
