using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.API.Requests.Volunteer.Update;

public record UpdateVolunteerSocialNetworkRequest(
    IEnumerable<SocialNetworksDto> SocialNetworks);