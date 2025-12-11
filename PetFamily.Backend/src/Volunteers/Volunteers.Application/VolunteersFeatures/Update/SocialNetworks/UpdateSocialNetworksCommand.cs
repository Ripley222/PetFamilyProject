using Application.Abstraction;
using Volunteers.Application.VolunteersFeatures.DTOs;

namespace Volunteers.Application.VolunteersFeatures.Update.SocialNetworks;

public record UpdateSocialNetworksCommand(
    Guid VolunteerId,
    IEnumerable<SocialNetworksDto> SocialNetworks) : ICommand;