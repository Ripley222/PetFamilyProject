using Application.Abstraction;
using Application.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.Update.Requisites;

public class UpdateRequisitesHandler(
    IVolunteersRepository repository,
    IValidator<UpdateRequisitesCommand> validator,
    ILogger<UpdateRequisitesHandler> logger) : ICommandHandler<Guid, UpdateRequisitesCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateRequisitesCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.GetErrors();

        var resultVolunteer = await repository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (resultVolunteer.IsFailure)
            return resultVolunteer.Error.ToErrorList();

        var requisites = command.Requisites.Select(x =>
            Requisite.Create(x.AccountNumber, x.Title, x.Description).Value);

        resultVolunteer.Value.UpdateRequisites(requisites);

        var saveResult = await repository.Save(resultVolunteer.Value, cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error.ToErrorList();

        logger.LogInformation("Update requisites volunteer with id {volunteerId}", command.VolunteerId);

        return saveResult.Value;
    }
}