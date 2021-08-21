using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Api.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<TContext>(
            this IHost host, Action<TContext, IServiceProvider> seeder, int retryCount = 0)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TContext>();
                context.Database.Migrate();
                seeder(context, scope.ServiceProvider);
            }            

            return host;
        }
    }
}