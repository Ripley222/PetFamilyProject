namespace PetFamily.Application.VolunteersFeatures.DTOs;

public record RequisitesDto
{
    public string AccountNumber { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}