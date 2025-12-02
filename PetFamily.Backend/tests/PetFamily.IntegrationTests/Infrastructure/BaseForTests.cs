using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Infrastructure;

namespace PetFamily.IntegrationTests.Infrastructure;

public class BaseForTests(WebTestsFactory webTestsFactory) : IClassFixture<WebTestsFactory>, IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase = webTestsFactory.ResetDatabaseAsync;

    private readonly IServiceProvider _services = webTestsFactory.Services;
    
    protected async Task<TResult> ExecuteHandlers<THandler, TResult>(
        Func<THandler, Task<TResult>> action) where THandler : notnull
    {
        await using var scope = _services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<THandler>();
        
        return await action(sut);
    }

    protected async Task<T> ExecuteInDatabase<T>(Func<ApplicationDbContext, Task<T>> action)
    {
        await using var scope = _services.CreateAsyncScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        return await action(dbContext);
    }
    
    protected async Task<T> ExecuteInMinio<T>(Func<IMinioClient, Task<T>> action)
    {
        await using var scope = _services.CreateAsyncScope();
        
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