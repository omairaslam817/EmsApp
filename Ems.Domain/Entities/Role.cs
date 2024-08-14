using Ems.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Domain.Entities
{
    public sealed class Role : Enumeration<Role>
    {
        public static readonly Role Registered = new(1, "Registered");
        private Role(int id, string name) 
            : base(id, name)
        {
        }
        public ICollection<Permission> Permissions { get; set; } //Hold Permission assignd to this role
        public ICollection<Member> Members { get; set; } //which member belong to which role,Now we can define which role has which Permissions
        //Memebers 
            // Roles
                //Permssions
                //uisng this hirechachy we can configre out which members has access to which Permission and that is going to tell which end point this Member can Access.


    }
}
