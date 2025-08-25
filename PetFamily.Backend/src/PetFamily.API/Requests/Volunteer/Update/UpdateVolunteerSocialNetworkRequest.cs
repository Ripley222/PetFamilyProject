using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Application.VolunteersFeatures.Update.SocialNetworks;

namespace PetFamily.API.Requests.Volunteer.Update;

public record UpdateVolunteerSocialNetworkRequest(
    IEnumerable<SocialNetworksDto> SocialNetworks)
{
    public UpdateSocialNetworksCommand ToCommand(Guid volunteerId) =>
        new(volunteerId,
            SocialNetworks);
}