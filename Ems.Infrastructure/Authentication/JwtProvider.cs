using Ems.Application.Abstractions.Jwt;
using Ems.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Infrastructure.Authentication
{
    public sealed class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;

        public JwtProvider(IOptions<JwtOptions> jwtOptions) //cant inject JwtOptions directly use IOptions
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string Generate(Member member)
        {
            var claims = new Claim[]
            { 
            new Claim(JwtRegisteredClaimNames.Sub,member.Id.ToString()), //adding claims
            new Claim(JwtRegisteredClaimNames.Email,member.Email.Value),

            }; //claims array for jwt
            var signingCredentials = new SigningCredentials( //siging jwt
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256

                );

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                null,
                DateTime.UtcNow.AddHours(1),
                signingCredentials); //signingCredentials take care of token creation
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token); //serilaize jwt into string
            return tokenValue;
        }
    }
}
