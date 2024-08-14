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
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(TableNames.Roles);

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Permissions) //A single permission can belong to more than 1 role,tell ef to create join table bw roles and permssions
                .WithMany()
                .UsingEntity<RolePermission>();

            builder.HasMany(x => x.Members) //A member can belong to more than 1 role,tell ef to create join table bw members and roles and also role and permssions
                .WithMany(x=> x.Roles);

            builder.HasData(Role.GetValues()); //Seed initial values for Permission
        }
    }
}
