using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Infrastructure;
using PetFamily.Infrastructure.Providers;

namespace PetFamily.IntegrationTests.Infrastructure;

public class BaseForTests(WebTestsFactory webTestsFactory) : IClassFixture<WebTestsFactory>, IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase = webTestsFactory.ResetDatabaseAsync;
    
    protected readonly IServiceProvider Services = webTestsFactory.Services;

    protected async Task<T> ExecuteInDatabase<T>(Func<ApplicationDbContext, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        return await action(dbContext);
    }
    
    protected async Task<T> ExecuteInMinio<T>(Func<IMinioClient, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<IMinioClient>();
        
        return await action(dbContext);
    }
        
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _resetDatabase();
    }
}