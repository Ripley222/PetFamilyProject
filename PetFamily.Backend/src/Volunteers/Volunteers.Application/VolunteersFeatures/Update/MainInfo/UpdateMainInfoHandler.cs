using Application.Abstraction;
using Application.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.Update.MainInfo;

public class UpdateMainInfoHandler(
    IVolunteersRepository repository,
    IValidator<UpdateMainInfoCommand> validator,
    ILogger<UpdateMainInfoHandler> logger) : ICommandHandler<Guid, UpdateMainInfoCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.GetErrors();

        var resultVolunteer = await repository.GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (resultVolunteer.IsFailure)
            return resultVolunteer.Error.ToErrorList();

        var fullName = FullName.Create(command.FirstName, command.MiddleName, command.LastName).Value;
        var emailAddress = EmailAddress.Create(command.EmailAddress).Value;
        var description = Description.Create(command.Description).Value;
        var yearsOfExperience = command.YearsOfExperience;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        resultVolunteer.Value.UpdateMainInfo(
            fullName,
            emailAddress,
            description,
            yearsOfExperience,
            phoneNumber);

        var saveResult = await repository.Save(resultVolunteer.Value, cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error.ToErrorList();

        logger.LogInformation("Updates main info volunteer with id {volunteerId}", command.VolunteerId);

        return saveResult.Value;
    }
}