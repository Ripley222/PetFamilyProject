using Application.Abstraction;
using Application.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.Delete.HardDelete;

public class HardDeleteVolunteerHandler(
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

        var deleteResult = await repository.Delete(resultVolunteer.Value, cancellationToken);
        if (deleteResult.IsFailure)
            return deleteResult.Error.ToErrorList();

        logger.LogInformation("Hard deleted volunteer with id {volunteerId}", command.VolunteerId);

        return deleteResult.Value;
    }
}