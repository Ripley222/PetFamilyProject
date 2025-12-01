using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;

public class DeletePetFileHandler(
    IFileProvider fileProvider,
    IVolunteersRepository repository,
    IValidator<DeletePetFileCommand> validator,
    ILogger<DeletePetFileHandler> logger)
{
    private const string BUCKET_NAME = "photos";
    
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
        if (filePath.IsFailure)
            return filePath.Error.ToErrorList();
        
        var removeResult = await fileProvider.RemoveFile(
            new FileData(filePath.Value, BUCKET_NAME), 
            cancellationToken);

        if (removeResult.IsFailure)
            return removeResult.Error;
        
        petExist.DeletePhoto(filePath.Value);
        
        await repository.Save(volunteerResult.Value, cancellationToken);
        
        logger.LogInformation("Deleting file {fileName}", command.FileName);

        return petExist.Id.Value;
    }
}