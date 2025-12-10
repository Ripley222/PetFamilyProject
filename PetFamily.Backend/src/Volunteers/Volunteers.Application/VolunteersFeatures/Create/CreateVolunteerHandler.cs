using Application.Abstraction;
using Application.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Volunteers.Application.Database;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.Create;

public class CreateVolunteerHandler(
    IVolunteersRepository volunteersRepository,
    IReadDbContext readDbContext,
    IValidator<CreateVolunteerCommand> validator,
    ILogger<CreateVolunteerHandler> logger) : ICommandHandler<Guid, CreateVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.GetErrors();

        var volunteerId = VolunteerId.New();
        var fullName = FullName.Create(command.FirstName, command.MiddleName, command.LastName).Value;

        var volunteerQuery = readDbContext.VolunteersRead;
        
        volunteerQuery = volunteerQuery.Where(v => v.FullName == fullName);
        if (volunteerQuery.Any())
            return Errors.VolunteerErrors.AlreadyExist().ToErrorList();
        
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

        var addResult = await volunteersRepository.Add(newVolunteer.Value, cancellationToken);
        if (addResult.IsFailure)
            return addResult.Error.ToErrorList();

        logger.LogInformation("Created volunteer with id {volunteerId}", volunteerId);
        
        return newVolunteer.Value.Id.Value;
    }
}