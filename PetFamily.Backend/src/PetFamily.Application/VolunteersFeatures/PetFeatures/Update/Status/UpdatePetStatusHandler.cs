using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.Update.Status;

public class UpdatePetStatusHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdatePetStatusCommand> validator,
    ILogger<UpdatePetStatusHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetStatusCommand command,
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

        var petExistResult = volunteerExistResult.Value.GetPetById(petId);
        if (petExistResult.IsFailure)
            return petExistResult.Error.ToErrorList();
        
        var helpStatus = HelpStatus.Create(command.HelpStatus).Value;
        
        petExistResult.Value.UpdateStatus(helpStatus);
        
        var saveResult = await volunteersRepository.Save(volunteerExistResult.Value, cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error.ToErrorList();
        
        logger.LogInformation("Updated status for pet with id {petId}", petId.Value);

        return petId.Value;
    }
}