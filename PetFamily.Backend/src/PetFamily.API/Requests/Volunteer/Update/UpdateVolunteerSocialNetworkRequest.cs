using PetFamily.Application.VolunteersFeatures.DTOs;

namespace PetFamily.API.Requests.Volunteer.Update;

public record UpdateVolunteerSocialNetworkRequest(
    IEnumerable<SocialNetworksDto> SocialNetworks);