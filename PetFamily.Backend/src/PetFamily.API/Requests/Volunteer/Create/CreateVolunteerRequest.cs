using PetFamily.Application.VolunteersFeatures.Create;
using PetFamily.Application.VolunteersFeatures.DTOs;

namespace PetFamily.API.Requests.Volunteer.Create;

public record CreateVolunteerRequest(
    string FirstName,
    string MiddleName,
    string LastName,
    string EmailAddress,
    string Description,
    int YearsOfExperience,
    string PhoneNumber,
    IEnumerable<RequisitesDto> Requisites,
    IEnumerable<SocialNetworksDto> SocialNetworks)
{
    public CreateVolunteerCommand ToCommand() =>
        new(FirstName,
            MiddleName,
            LastName,
            EmailAddress,
            Description,
            YearsOfExperience,
            PhoneNumber,
            Requisites,
            SocialNetworks);
}