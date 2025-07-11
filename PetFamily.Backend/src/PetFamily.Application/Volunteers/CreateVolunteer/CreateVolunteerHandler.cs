﻿using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler(
    IVolunteersRepository volunteersRepository, 
    IValidator<CreateVolunteerCommand> validator)
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
        var fullNameResult = FullName.Create(command.FirstName, command.MiddleName, command.LastName).Value;

        var existingVolunteer = await volunteersRepository
            .GetByFullName(fullNameResult, cancellationToken);

        if (existingVolunteer.IsSuccess)
            return Errors.Volunteer.AlreadyExist().ToErrorList();
        
        var emailAddressResult = EmailAddress.Create(command.EmailAddress).Value;
        var descriptionResult = Description.Create(command.Description).Value;
        var yearsOfExperience = command.YearsOfExperience;
        var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber).Value;
        
        var requisites = command.Requisites
            .Select(dto => Requisite.Create(dto.AccountNumber, dto.Title, dto.Description).Value)
            .ToList();
        
        var socialNetworks = command.SocialNetworks
            .Select(dto => SocialNetwork.Create(dto.Title, dto.Link).Value)
            .ToList();
        
        var newVolunteer = Volunteer.Create(
            volunteerId, 
            fullNameResult, 
            emailAddressResult, 
            descriptionResult, 
            yearsOfExperience, 
            phoneNumberResult,
            requisites,
            socialNetworks);
        
        if (newVolunteer.IsFailure)
            return newVolunteer.Error.ToErrorList();

        await volunteersRepository.Add(newVolunteer.Value, cancellationToken);

        return newVolunteer.Value.Id.Value;
    }
}