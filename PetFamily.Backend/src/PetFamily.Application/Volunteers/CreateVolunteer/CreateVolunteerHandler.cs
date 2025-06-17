using CSharpFunctionalExtensions;
using PetFamily.Application.Volunteers.Commands;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    
    public async Task<Result<Guid, Error>> Handler(
        CreateVolunteerCommand command, CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.New();
        
        var fullNameResult = FullName.Create(command.FirstName, command.MiddleName, command.LastName);
        if (fullNameResult.IsFailure)
            return fullNameResult.Error;

        var existingVolunteer = await _volunteersRepository.GetByFullName(fullNameResult.Value, cancellationToken);

        if (existingVolunteer.IsSuccess)
            return Errors.Volunteer.AlreadyExist();
        
        var emailAddressResult = EmailAddress.Create(command.EmailAddress);
        if (emailAddressResult.IsFailure)
            return emailAddressResult.Error;
        
        var descriptionResult = Description.Create(command.Description);
        if (descriptionResult.IsFailure)
            return descriptionResult.Error;
        
        var yearsOfExperience = command.YearsOfExperience;
        
        var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber);
        if (phoneNumberResult.IsFailure)
            return phoneNumberResult.Error;

        var requisites = command.Requisites
            .Select(dto => Requisites.Create(dto.AccountNumber, dto.Title, dto.Description).Value)
            .ToList();

        var requisitesList = new RequisitesList(requisites);
        
        var socialNetworks = command.SocialNetworks
            .Select(dto => SocialNetwork.Create(dto.Title, dto.Link).Value)
            .ToList();

        var socialNetworksList = new SocialNetworksList(socialNetworks);
        
        var newVolunteer = Volunteer.Create(
            volunteerId, 
            fullNameResult.Value, 
            emailAddressResult.Value, 
            descriptionResult.Value, 
            yearsOfExperience, 
            phoneNumberResult.Value,
            requisitesList,
            socialNetworksList);
        
        if (newVolunteer.IsFailure)
            return newVolunteer.Error;

        await _volunteersRepository.Add(newVolunteer.Value, cancellationToken);

        return newVolunteer.Value.Id.Value;
    }
}