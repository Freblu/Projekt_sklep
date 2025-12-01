using Microsoft.EntityFrameworkCore;
using PartsCom.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace PartsCom.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        scope.ApplyMigration<PartsComDbContext>();
    }
    
    private static void ApplyMigration<TDbContext>(this IServiceScope scope) where TDbContext : DbContext
    {
        using TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        dbContext.Database.Migrate();
    }
}
