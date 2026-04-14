using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PartsCom.Domain.Entities;
using PartsCom.Infrastructure.Database;

namespace PartsCom.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        scope.ApplyMigration<PartsComDbContext>();
    }

    public static async Task ApplyMigrationsAndSeedAsync(this IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        PartsComDbContext dbContext = scope.ServiceProvider.GetRequiredService<PartsComDbContext>();
        await dbContext.Database.MigrateAsync();
        await DataSeeder.SeedAsync(dbContext);
    }

    private static void ApplyMigration<TDbContext>(this IServiceScope scope) where TDbContext : DbContext
    {
        using TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        dbContext.Database.Migrate();
    }

    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        PartsComDbContext dbContext = scope.ServiceProvider.GetRequiredService<PartsComDbContext>();
        await DataSeeder.SeedAsync(dbContext);
    }
}
