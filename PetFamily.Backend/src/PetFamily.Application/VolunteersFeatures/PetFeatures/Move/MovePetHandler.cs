using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.Move;

public class MovePetHandler(
    IVolunteersRepository repository,
    IValidator<MovePetCommand> validator,
    ILogger<MovePetHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Handle(
        MovePetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.GetErrors();
        
        var volunteerResult = await repository.GetById(
            VolunteerId.Create(command.VolunteerId), cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petExit = volunteerResult.Value.Pets
            .FirstOrDefault(p => p.Id == PetId.Create(command.PetId));
        
        if (petExit is null)
            return Errors.Pet.NotFound().ToErrorList();

        var result = volunteerResult.Value
            .MovePet(petExit, Position.Create(command.NewPosition).Value);

        if (result.IsFailure)
            return result.Error.ToErrorList();

        await repository.Save(volunteerResult.Value, cancellationToken);
        
        logger.LogInformation("Pet with id {petId} moved by volunteer with id {volunteerId}",
            command.PetId,
            command.VolunteerId);
        
        return petExit.Id.Value;
    }
}