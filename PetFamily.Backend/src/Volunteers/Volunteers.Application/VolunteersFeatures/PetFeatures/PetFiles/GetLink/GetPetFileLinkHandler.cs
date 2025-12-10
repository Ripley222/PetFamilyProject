using Application.Abstraction;
using Application.Extensions;
using Application.Options;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Volunteers.Application.FileProvider;
using Volunteers.Application.Providers;
using Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.Add.AddManyFiles;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.GetLink;

public class GetPetFileLinkHandler(
    IVolunteersRepository repository,
    IFileProvider fileProvider,
    IMinioBucketOptions bucketOptions,
    IValidator<GetPetFileLinkCommand> validator,
    ILogger<AddPetFilesHandler> logger) : ICommandHandler<string, GetPetFileLinkCommand>
{
    public async Task<Result<string, ErrorList>> Handle(
        GetPetFileLinkCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.GetErrors();
        
        var volunteerResult = await repository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petExist = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == PetId.Create(command.PetId));
        if (petExist is null)
            return Errors.PetErrors.NotFound().ToErrorList();

        var filePath = FilePath.ParseOrGenerate(command.FileName);
        
        var fileData = new FileData(
            filePath, 
            bucketOptions.BucketPhotos);

        var result = await fileProvider.GetFileLink(fileData, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        logger.LogInformation("Getting file link {fileName}", command.FileName);

        return result.Value;
    }
}