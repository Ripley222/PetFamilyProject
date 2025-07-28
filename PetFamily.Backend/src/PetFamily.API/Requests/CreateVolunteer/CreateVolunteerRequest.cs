using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.API.Requests.CreateVolunteer;

public record CreateVolunteerRequest(
    string FirstName, 
    string MiddleName,
    string LastName, 
    string EmailAddress, 
    string Description, 
    int YearsOfExperience, 
    string PhoneNumber,
    IEnumerable<RequisitesDto> Requisites,
    IEnumerable<SocialNetworksDto> SocialNetworks);