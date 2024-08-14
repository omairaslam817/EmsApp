using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ems.Domain.Entities;
using Ems.Persistence;
using Microsoft.EntityFrameworkCore;
namespace Ems.Infrastructure.Authrization
{
    internal class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _context;

        public PermissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HashSet<string>> GetPermissionsAsync(Guid memberId) //fetch permission with spcified id
        {
         ICollection<Role> [] roles = await _context.Set<Member>() //selcet role with permssions,User can belong to multiple roles,we get back icollection of roles and make array [] of that ,we have array of arrays,SO flatten this to get Hashset
                .Include(m=> m.Roles)
                    .ThenInclude(m=> m.Permissions)
                .Where(m => m.Id == memberId)
                .Select(x => x.Roles)
                .ToArrayAsync(); //and roles cotains permissions
            return roles
                .SelectMany(x => x)
                .SelectMany(m => m.Permissions)
                .Select(m => m.Name)
                .ToHashSet();
        }
    }
}
