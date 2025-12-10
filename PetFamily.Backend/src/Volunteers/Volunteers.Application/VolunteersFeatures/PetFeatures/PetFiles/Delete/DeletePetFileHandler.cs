using Application.Abstraction;
using Application.Extensions;
using Application.Options;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Volunteers.Application.FileProvider;
using Volunteers.Application.Providers;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;

public class DeletePetFileHandler(
    IFileProvider fileProvider,
    IVolunteersRepository repository,
    IMinioBucketOptions bucketOptions,
    IValidator<DeletePetFileCommand> validator,
    ILogger<DeletePetFileHandler> logger) : ICommandHandler<Guid, DeletePetFileCommand>
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
            return Errors.PetErrors.NotFound().ToErrorList();
        
        var filePath = FilePath.ParseOrGenerate(command.FileName);
        
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