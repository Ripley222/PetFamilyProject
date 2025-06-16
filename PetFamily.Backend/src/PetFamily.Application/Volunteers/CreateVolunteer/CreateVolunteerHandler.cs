using CSharpFunctionalExtensions;
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
        CreateVolunteerRequest request, CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.New();
        
        var fullNameResult = FullName.Create(request.FirstName, request.MiddleName, request.LastName);
        if (fullNameResult.IsFailure)
            return fullNameResult.Error;

        var existingVolunteer = await _volunteersRepository.GetByFullName(fullNameResult.Value, cancellationToken);

        if (existingVolunteer.IsSuccess)
            return Errors.Volunteer.AlreadyExist();
        
        var emailAddressResult = EmailAddress.Create(request.EmailAddress);
        if (emailAddressResult.IsFailure)
            return emailAddressResult.Error;
        
        var descriptionResult = Description.Create(request.Description);
        if (descriptionResult.IsFailure)
            return descriptionResult.Error;
        
        var yearsOfExperience = request.YearsOfExperience;
        
        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
        if (phoneNumberResult.IsFailure)
            return phoneNumberResult.Error;

        var requisites = request.Requisites
            .Select(dto => Requisites.Create(dto.AccountNumber, dto.Title, dto.Description).Value)
            .ToList();
        
        var requisitesList = RequisitesList.Create(requisites);
        
        var socialNetworks = request.SocialNetworks
            .Select(dto => SocialNetwork.Create(dto.Title, dto.Link).Value)
            .ToList();
        
        var socialNetworksList = SocialNetworksList.Create(socialNetworks);
        
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