using Ems.Domain.Entities;
using Ems.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Persistence.Configurations
{
    internal class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(TableNames.Permissions);
            builder.HasKey(p => p.Id);

            //Seed initial value for  Permission
         IEnumerable<Permission> permessions =   Enum.GetValues<Domain.Enums.Permission>() //foreach vaue of permission enum ,create seprate instance of permission entity
                .Select(p => new Permission
                {
                    Id = (int) p,
                    Name = p.ToString()
                });
            builder.HasData(permessions); //seeed permission values using ef migration
        }
    }
}
