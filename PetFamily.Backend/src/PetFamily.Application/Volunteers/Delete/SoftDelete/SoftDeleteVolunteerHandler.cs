using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.Delete.HardDelete;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Delete.SoftDelete;

public class SoftDeleteVolunteerHandler(
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
        
        resultVolunteer.Value.Delete();
        
        var result = await repository.Save(resultVolunteer.Value, cancellationToken);
        
        logger.LogInformation("Soft deleted volunteer with id {volunteerId}", command.VolunteerId);
        
        return result;
    }
}