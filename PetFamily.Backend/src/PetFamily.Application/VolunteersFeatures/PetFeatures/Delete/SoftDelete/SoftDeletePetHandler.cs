using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.Delete.SoftDelete;

public class SoftDeletePetHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<DeletePetCommand> validator,
    ILogger<SoftDeletePetHandler> logger)
{
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
        
        petResult.Value.Delete();
        
        var saveResult = await volunteersRepository.Save(volunteerExistResult.Value, cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error.ToErrorList();
        
        logger.LogInformation("Soft deleted pet with id {petId}", petId.Value);

        return petId.Value;
    }
}