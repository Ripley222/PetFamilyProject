using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler(
    IVolunteersRepository volunteersRepository, 
    IValidator<CreateVolunteerCommand> validator,
    ILogger<CreateVolunteerHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Handler(
        CreateVolunteerCommand command,
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

        var volunteerId = VolunteerId.New();
        var fullName = FullName.Create(command.FirstName, command.MiddleName, command.LastName).Value;

        var existingVolunteer = await volunteersRepository
            .GetByFullName(fullName, cancellationToken);

        if (existingVolunteer.IsSuccess)
            return Errors.Volunteer.AlreadyExist().ToErrorList();
        
        var emailAddress = EmailAddress.Create(command.EmailAddress).Value;
        var description = Description.Create(command.Description).Value;
        var yearsOfExperience = command.YearsOfExperience;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        
        var requisites = command.Requisites
            .Select(dto => Requisite.Create(dto.AccountNumber, dto.Title, dto.Description).Value)
            .ToList();
        
        var socialNetworks = command.SocialNetworks
            .Select(dto => SocialNetwork.Create(dto.Title, dto.Link).Value)
            .ToList();
        
        var newVolunteer = Volunteer.Create(
            volunteerId, 
            fullName, 
            emailAddress, 
            description, 
            yearsOfExperience, 
            phoneNumber,
            requisites,
            socialNetworks);
        
        if (newVolunteer.IsFailure)
            return newVolunteer.Error.ToErrorList();

        await volunteersRepository.Add(newVolunteer.Value, cancellationToken);

        logger.LogInformation("Created volunteer with id {volunteerId}", volunteerId);
        
        return newVolunteer.Value.Id.Value;
    }
}