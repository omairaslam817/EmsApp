using Ems.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Permission = Ems.Domain.Enums.Permission;

namespace Ems.Persistence.Configurations
{
    internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(x => new {x.RoleId,x.PermissionId});//defone composte key using ef ,return new object,because dont want to allow role have same permision multiple time
//Configure which ROle have access to which Permission from code.apply changes,by createing new Ef Migration
            builder.HasData(
                Create(Role.Registered, Permission.ReadMember));//create () to define new role permssion instance
            builder.HasData(
                Create(Role.Registered, Permission.UpdateMember));
        }
        private static RolePermission Create(Role role,Permission permission)
        {
            return new RolePermission
            {
                RoleId = role.Id,
                PermissionId = (int)permission, //to int coz 
            };
        }
    }
}
