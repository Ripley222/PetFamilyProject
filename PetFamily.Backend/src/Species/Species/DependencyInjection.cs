using Application.Abstraction;
using Framework;
using Microsoft.Extensions.DependencyInjection;
using Species.Database;

namespace Species;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddScoped<SpeciesDbContext>();

        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes =>
                classes.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes =>
                classes.AssignableToAny(typeof(IQueryHandler<,>), typeof(IQueryHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.AddEndpoints(assembly);

        return services;
    }
}