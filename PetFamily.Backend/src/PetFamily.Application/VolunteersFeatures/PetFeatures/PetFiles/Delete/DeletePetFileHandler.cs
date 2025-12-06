using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Options;
using PetFamily.Application.Providers;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;

public class DeletePetFileHandler(
    IFileProvider fileProvider,
    IVolunteersRepository repository,
    IMinioBucketOptions bucketOptions,
    IValidator<DeletePetFileCommand> validator,
    ILogger<DeletePetFileHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeletePetFileCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.GetErrors();
        
        var volunteerResult = await repository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petExist = volunteerResult.Value.Pets.FirstOrDefault(
            p => p.Id == PetId.Create(command.PetId));

        if (petExist is null)
            return Errors.Pet.NotFound().ToErrorList();
        
        var extension = Path.GetExtension(command.FileName);
        var fileName = extension == string.Empty 
            ? command.FileName
            : command.FileName.Replace(extension, string.Empty);
        
        var filePath = FilePath.Create(Guid.Parse(fileName), extension);
        
        var removeResult = await fileProvider.RemoveFile(
            new FileData(filePath, bucketOptions.BucketPhotos), 
            cancellationToken);

        if (removeResult.IsFailure)
            return removeResult.Error;
        
        petExist.DeletePhoto(filePath);
        
        await repository.Save(volunteerResult.Value, cancellationToken);
        
        logger.LogInformation("Deleting file {fileName}", command.FileName);

        return petExist.Id.Value;
    }
}