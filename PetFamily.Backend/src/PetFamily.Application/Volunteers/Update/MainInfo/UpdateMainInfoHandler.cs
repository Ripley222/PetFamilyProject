using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.MainInfo;

public class UpdateMainInfoHandler(
    IVolunteersRepository repository,
    IValidator<UpdateMainInfoCommand> validator,
    ILogger<UpdateMainInfoHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            var validationErrors = validationResult.Errors;

            var errors = validationErrors.Select(validationError
                => Error.Validation(
                    validationError.ErrorCode,
                    validationError.ErrorMessage,
                    validationError.PropertyName));

            return new ErrorList(errors);
        }

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

        var result = await repository.Save(resultVolunteer.Value, cancellationToken);
        
        logger.LogInformation("Updates main info volunteer with id {volunteerId}", resultVolunteer.Value.Id);
        
        return result;
    }
}