using PetFamily.Application.VolunteersFeatures.DTOs;

namespace PetFamily.Application.VolunteersFeatures.Create;

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