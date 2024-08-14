using Ems.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Application.Abstractions.Jwt
{
    public interface IJwtProvider
    {
        string Generate(Member member);
    }
}
