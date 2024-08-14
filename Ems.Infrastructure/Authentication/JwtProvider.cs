using Ems.Application.Abstractions.Jwt;
using Ems.Domain.Entities;
using Ems.Infrastructure.Authrization;
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
        private readonly IPermissionService _permissionService;

        public JwtProvider(IOptions<JwtOptions> jwtOptions, IPermissionService permissionService) //cant inject JwtOptions directly use IOptions
        {
            _jwtOptions = jwtOptions.Value;
            _permissionService = permissionService;

        }

        public async Task<string> GenerateAsync(Member member)
        {
            var claims = new List<Claim>
            { 
            new Claim(JwtRegisteredClaimNames.Sub,member.Id.ToString()), //adding claims
            new Claim(JwtRegisteredClaimNames.Email,member.Email.Value),

            };
            //For Jwt Custom Claims
            HashSet<string> permissions = await _permissionService.GetPermissionsAsync(member.Id); //contains memeber permission

            foreach (var permission in permissions) //add each permission as new claim in claims collection
            {
                claims.Add(new(CustomClaims.Permissions, permission)); //adding permission as claim inside Jwt
                //add constant inside CustomClaims.cs for custom claim name for our permissions,that will hold the name of the claim for our permissions
            }
            //claims array for jwt
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
