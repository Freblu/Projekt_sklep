using Minio;
using Microsoft.EntityFrameworkCore;
using PartsCom.Application.Interfaces;
using PartsCom.Infrastructure.Database;
using PartsCom.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using PartsCom.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartsCom.Infrastructure;

public static class Installer
{
    public static IServiceCollection InstallInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddHttpContextAccessor();

        services.AddDbContext<PartsComDbContext>(options =>
            options
                .UseSqlServer(
                    configuration.GetConnectionString("PartsCom") ??
                    throw new InvalidOperationException("Connection string 'PartsCom' not found."),
                    cfg => cfg.MigrationsHistoryTable(HistoryRepository.DefaultTableName)
                )
                .UseSnakeCaseNamingConvention()
        );

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<PartsComDbContext>());

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPasswordHashService, PasswordHashService>();
        services.AddSingleton<IJwtService, JwtService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();


        var uri = new Uri(Environment.GetEnvironmentVariable("PARTSCOM-MINIO_URI") ?? "");

        services.AddSingleton<IMinioClient>(_ => new MinioClient()
            .WithEndpoint(uri.Authority)
            .WithCredentials(
                Environment.GetEnvironmentVariable("PARTSCOM-MINIO_ACCESSKEY"),
                Environment.GetEnvironmentVariable("PARTSCOM-MINIO_SECRETKEY"))
            .WithSSL(uri.Scheme == "https")
            .Build());

        services.AddScoped<IFileStorageService, MinioStorageService>();

        return services;
    }
}
