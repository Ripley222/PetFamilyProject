using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.Providers;
using PetFamily.Application.SpeciesFeatures;
using PetFamily.Application.VolunteersFeatures;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IFileProvider, MinioProvider>();
        services.AddMinio(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddMinio(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(
            configuration.GetSection(MinioOptions.MINIO));
        
        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");
            
            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });
        
        return services;
    }
}