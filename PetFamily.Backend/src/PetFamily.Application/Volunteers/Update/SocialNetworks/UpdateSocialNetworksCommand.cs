using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.Application.Volunteers.Update.SocialNetworks;

public record UpdateSocialNetworksCommand(
    Guid VolunteerId,
    IEnumerable<SocialNetworksDto> SocialNetworks);