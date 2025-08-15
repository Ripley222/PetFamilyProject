using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.Delete.HardDelete;

public class HardDeleteVolunteerHandler(
    IVolunteersRepository repository,
    IValidator<DeleteVolunteerCommand> validator,
    ILogger<HardDeleteVolunteerHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.GetErrors();
        }

        var resultVolunteer = await repository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (resultVolunteer.IsFailure)
            return resultVolunteer.Error.ToErrorList();
        
        var result = await repository.Delete(resultVolunteer.Value, cancellationToken);
        
        logger.LogInformation("Hard deleted volunteer with id {volunteerId}", command.VolunteerId);
        
        return result;
    }
}