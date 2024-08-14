using Ems.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace Ems.WebApi.Server.OptionsSetup
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private const string SectionName = "Jwt";
        private readonly IConfiguration _configuration;
        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options) //set propties on Jwt Option instance
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
