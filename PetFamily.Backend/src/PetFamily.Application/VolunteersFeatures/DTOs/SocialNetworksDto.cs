namespace PetFamily.Application.VolunteersFeatures.DTOs;

public record SocialNetworksDto
{
    public string Title { get; init; } = string.Empty;
    public string Link { get; init; } = string.Empty;
}