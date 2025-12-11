/*using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Minio;
using Npgsql;
using PetFamily.API;
using Respawn;
using Testcontainers.Minio;
using Testcontainers.PostgreSql;
using Volunteers.Application.Database;

namespace PetFamily.IntegrationTests.Infrastructure;

public class WebTestsFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithName("tests_postgres")
        .WithImage("postgres")
        .WithHostname("postgres")
        .WithDatabase("pet_family_tests_db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly MinioContainer _minioContainer = new MinioBuilder()
        .WithName("tests_minio")
        .WithImage("minio/minio:latest")
        .WithUsername("minioadmin")
        .WithPassword("minioadmin")
        .Build();

    private Respawner _respawner = null!;
    private DbConnection _dbConnection = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ApplicationDbContext>();
            services.RemoveAll<IMinioClient>();

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Database"] = _dbContainer.GetConnectionString()
                });

            var build = configurationBuilder.Build();

            services.AddScoped<IReadDbContext, ApplicationDbContext>(_ =>
                new ApplicationDbContext(build));

            services.AddScoped<ApplicationDbContext>(_ =>
                new ApplicationDbContext(build));

            services.AddMinio(options =>
            {
                var uri = new Uri(_minioContainer.GetConnectionString());
                
                options.WithEndpoint(uri);
                options.WithCredentials(_minioContainer.GetAccessKey(), _minioContainer.GetSecretKey());
                options.WithSSL(false);
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        await _minioContainer.StartAsync();

        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await _dbConnection.OpenAsync();

        await InitializeRespawner();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();

        await _dbConnection.CloseAsync();
        await _dbConnection.DisposeAsync();

        await _minioContainer.StopAsync();
        await _minioContainer.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    private async Task InitializeRespawner()
    {
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"]
            });
    }
}*/