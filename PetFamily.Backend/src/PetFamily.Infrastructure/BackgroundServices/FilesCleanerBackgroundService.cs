using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;

namespace PetFamily.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    IMassageChannel<IEnumerable<FileData>> massageChannel,
    ILogger<FilesCleanerBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("FilesCleanerBackgroundService is starting.");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = serviceScopeFactory.CreateAsyncScope();

            var fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();
            
            //чтение данных из Channel для удаления
            var fileInfos = await massageChannel.ReadAsync(stoppingToken);

            foreach (var fileInfo in fileInfos)
            {
                await fileProvider.RemoveFile(fileInfo, stoppingToken);
                
                logger.LogInformation(
                    "FilesCleanerBackgroundService deleting file with path {path}.", fileInfo.FilePath.Value);
            }
        }
        
        await Task.CompletedTask;
    }
}