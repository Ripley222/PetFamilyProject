namespace PetFamily.Application.VolunteersFeatures.GetWithPagination;

public record GetVolunteersCommand(
    int Page,
    int PageSize);