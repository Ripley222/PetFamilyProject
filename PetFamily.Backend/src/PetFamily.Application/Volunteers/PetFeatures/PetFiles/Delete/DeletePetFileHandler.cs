using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.PetFeatures.PetFiles.Delete;

public class DeletePetFileHandler(
    IFileProvider fileProvider,
    ILogger<DeletePetFileHandler> logger)
{
    public async Task<Result<string, ErrorList>> Handle(
        DeletePetFileCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await fileProvider.RemoveFile(
            new FileData(command.BucketName, command.FileName), 
            cancellationToken);
        
        logger.LogInformation("Deleting file {fileName}", command.FileName);

        return result;
    }
}