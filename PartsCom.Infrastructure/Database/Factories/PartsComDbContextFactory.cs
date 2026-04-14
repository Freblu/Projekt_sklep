using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

namespace PartsCom.Infrastructure.Database.Factories;

internal sealed class PartsComDbContextFactory : IDesignTimeDbContextFactory<PartsComDbContext>
{
    private const string InfrastructureDirectoryName = "PartsCom.Infrastructure";

    public PartsComDbContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", InfrastructureDirectoryName))
            .Build();

        string connectionString = configuration.GetConnectionString("PartsCom");

        DbContextOptionsBuilder<PartsComDbContext> optionsBuilder = new();

        optionsBuilder
            .UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName)
            )
            .UseSnakeCaseNamingConvention();

        return new PartsComDbContext(optionsBuilder.Options);
    }
}

