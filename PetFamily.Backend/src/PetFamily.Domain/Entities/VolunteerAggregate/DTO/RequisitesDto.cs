namespace PetFamily.Domain.Entities.VolunteerAggregate.DTO;

public record RequisitesDto
{
    public string AccountNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}