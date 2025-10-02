using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Delete.SoftDelete;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.Delete.HardDelete;

public class HardDeletePetHandler(
    IFileProvider fileProvider,
    IVolunteersRepository volunteersRepository,
    IValidator<DeletePetCommand> validator,
    ILogger<SoftDeletePetHandler> logger)
{
    private const string BUCKET_NAME = "photos";

    public async Task<Result<Guid, ErrorList>> Handle(
        DeletePetCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid is false)
            return validationResult.GetErrors();

        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerExistResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerExistResult.IsFailure)
            return volunteerExistResult.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);

        var petResult = volunteerExistResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var removePetResult = volunteerExistResult.Value.RemovePet(petResult.Value);
        if (removePetResult.IsFailure)
            return removePetResult.Error.ToErrorList();

        if (petResult.Value.Files.Count is not 0)
        {
            var removeFilesResult = await fileProvider.RemoveFiles(
                new DeleteFilesData(
                    petResult.Value.Files.Select(f => f.Value),
                    BUCKET_NAME),
                cancellationToken);

            if (removeFilesResult.IsFailure)
                return removeFilesResult.Error;
            
            logger.LogInformation("Deleted pet files with pet id {petId}", petId.Value);
        }

        var saveResult = await volunteersRepository.Save(volunteerExistResult.Value, cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error.ToErrorList();

        logger.LogInformation("Hard deleted pet with id {petId}", petId.Value);

        return petId.Value;
    }
}