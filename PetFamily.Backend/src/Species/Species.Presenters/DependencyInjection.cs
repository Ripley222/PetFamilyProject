using Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Species.Contracts;

namespace Species.Presenters;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesModule(this IServiceCollection services)
    {
        services.AddScoped<ISpeciesContract, SpeciesContract>();
        services.AddSpeciesServices();

        return services;
    }
}