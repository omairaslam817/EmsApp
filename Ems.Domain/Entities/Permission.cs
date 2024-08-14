using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Domain.Entities
{
    public class Permission //Ef core to hold actuall permission in table
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
