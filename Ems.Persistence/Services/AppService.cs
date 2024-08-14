using Ems.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Persistence.Services
{
    public static class AppService
    {
        public static void ApplyMigration(this IApplicationBuilder application)
        {
            using var scope = application.ApplicationServices.CreateAsyncScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dataContext.Database.Migrate();
        }
    }
}
