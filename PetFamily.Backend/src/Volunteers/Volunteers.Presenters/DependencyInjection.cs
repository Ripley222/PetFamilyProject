using Infrastructure.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volunteers.Application;
using Volunteers.Infrastructure.Postgres;

namespace Volunteers.Presenters;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructurePostgres(configuration);
        services.AddInfrastructureS3(configuration);
        
        return services;
    }
}