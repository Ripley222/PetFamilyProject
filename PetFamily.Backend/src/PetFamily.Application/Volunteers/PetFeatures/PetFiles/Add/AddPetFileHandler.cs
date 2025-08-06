using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.PetFeatures.PetFiles.Add;

public class AddPetFileHandler(
    IFileProvider fileProvider,
    ILogger<AddPetFileHandler> logger)
{
    public async Task<Result<string, ErrorList>> Handle(
        AddPetFileCommand command,
        CancellationToken cancellationToken = default)
    {
        var fileData = new StreamFileData(
            command.Stream,
            new FileData(
                command.BucketName,
                command.FileName));
        
        logger.LogInformation("Uploading file {fileName}", command.FileName);
        
        return await fileProvider.UploadFile(fileData, cancellationToken);
    }
}