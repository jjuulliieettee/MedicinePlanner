using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MedicinePlanner.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IApplicationBuilder UseAutoMigrateDatabase<TDbContext>(this IApplicationBuilder builder)
            where TDbContext : DbContext
        {
            using var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            serviceScope.ServiceProvider.GetService<TDbContext>()?.Database.Migrate();

            return builder;
        }

    }
}
