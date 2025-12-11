using Volunteers.Application.VolunteersFeatures.DTOs;
using Volunteers.Application.VolunteersFeatures.Update.SocialNetworks;

namespace Volunteers.Presenters.Requests.Volunteer.Update;

public record UpdateVolunteerSocialNetworkRequest(
    IEnumerable<SocialNetworksDto> SocialNetworks)
{
    public UpdateSocialNetworksCommand ToCommand(Guid volunteerId) =>
        new(volunteerId,
            SocialNetworks);
}