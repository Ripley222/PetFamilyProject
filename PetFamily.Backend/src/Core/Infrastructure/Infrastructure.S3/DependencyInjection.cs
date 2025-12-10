using Application.Messaging;
using Application.Options;
using Infrastructure.S3.MassageChannels;
using Infrastructure.S3.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using Volunteers.Application.FileProvider;
using Volunteers.Application.Providers;

namespace Infrastructure.S3;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureS3(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMinio(configuration);

        services.AddScoped<IFileProvider, MinioProvider>();
        
        services.AddSingleton<IMassageChannel<FileData>, 
            InMemoryCleanerMassageChannel<FileData>>();
        
        services.AddSingleton<IMassageChannel<IEnumerable<FileData>>, 
            InMemoryCleanerMassageChannel<IEnumerable<FileData>>>();
        
        return services;
    }
    
    private static IServiceCollection AddMinio(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(
            configuration.GetSection(MinioOptions.MINIO));
        
        services.AddSingleton<IMinioBucketOptions>(sp =>
            sp.GetRequiredService<IOptions<MinioOptions>>().Value.BucketOptions);
        
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