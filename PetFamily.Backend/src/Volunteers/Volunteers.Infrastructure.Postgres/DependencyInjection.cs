using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volunteers.Application.Database;
using Volunteers.Application.VolunteersFeatures;

namespace Volunteers.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructurePostgres(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<VolunteerDbContext>();
        services.AddScoped<IReadDbContext, VolunteerDbContext>();
        
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        
        return services;
    }
}