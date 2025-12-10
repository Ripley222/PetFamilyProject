using Application.Abstraction;
using Application.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures.Delete.HardDelete;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.Delete.SoftDelete;

public class SoftDeleteVolunteerHandler(
    IVolunteersRepository repository,
    IValidator<DeleteVolunteerCommand> validator,
    ILogger<HardDeleteVolunteerHandler> logger) : ICommandHandler<Guid, DeleteVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.GetErrors();

        var resultVolunteer = await repository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (resultVolunteer.IsFailure)
            return resultVolunteer.Error.ToErrorList();
        
        resultVolunteer.Value.Delete();
        
        var saveResult = await repository.Save(resultVolunteer.Value, cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error.ToErrorList();
        
        logger.LogInformation("Soft deleted volunteer with id {volunteerId}", command.VolunteerId);
        
        return saveResult.Value;
    }
}