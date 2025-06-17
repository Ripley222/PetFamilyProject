using PetFamily.Application.Volunteers.DTO;

namespace PetFamily.Application.Volunteers.Commands;

public record CreateVolunteerCommand(
    string FirstName, 
    string MiddleName,
    string LastName, 
    string EmailAddress, 
    string Description, 
    int YearsOfExperience, 
    string PhoneNumber,
    IEnumerable<RequisitesDto> Requisites,
    IEnumerable<SocialNetworksDto> SocialNetworks);