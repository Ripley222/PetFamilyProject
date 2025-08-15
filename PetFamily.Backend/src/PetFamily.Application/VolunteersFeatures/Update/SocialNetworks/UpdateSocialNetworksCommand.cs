using PetFamily.Application.VolunteersFeatures.DTOs;

namespace PetFamily.Application.VolunteersFeatures.Update.SocialNetworks;

public record UpdateSocialNetworksCommand(
    Guid VolunteerId,
    IEnumerable<SocialNetworksDto> SocialNetworks);