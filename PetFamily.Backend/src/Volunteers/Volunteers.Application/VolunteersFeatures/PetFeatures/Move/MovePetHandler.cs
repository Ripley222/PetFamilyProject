using Application.Abstraction;
using Application.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.Move;

public class MovePetHandler(
    IVolunteersRepository repository,
    IValidator<MovePetCommand> validator,
    ILogger<MovePetHandler> logger) : ICommandHandler<Guid, MovePetCommand>
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
            return Errors.PetErrors.NotFound().ToErrorList();

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