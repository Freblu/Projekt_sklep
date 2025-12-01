using FluentValidation;
using PartsCom.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace PartsCom.Application;

public static class Installer
{
    public static IServiceCollection InstallApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(Installer).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });
        
        services.AddValidatorsFromAssemblies([typeof(Installer).Assembly], includeInternalTypes: true);
        
        return services;
    }
}
