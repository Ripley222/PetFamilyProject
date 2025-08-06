using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Application.Volunteers.PetFeatures.PetFiles.Add;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.PetFeatures.PetFiles.GetLink;

public class GetPetFileLinkHandler(
    IFileProvider fileProvider,
    ILogger<AddPetFileHandler> logger)
{
    public async Task<Result<string, ErrorList>> Handle(
        GetPetFileLinkCommand linkCommand,
        CancellationToken cancellationToken = default)
    {
        var fileData = new FileData(
                linkCommand.BucketName,
                linkCommand.FileName);
        
        logger.LogInformation("Getting file link {fileName}", linkCommand.FileName);
        
        return await fileProvider.GetFileLink(fileData, cancellationToken);
    }
}