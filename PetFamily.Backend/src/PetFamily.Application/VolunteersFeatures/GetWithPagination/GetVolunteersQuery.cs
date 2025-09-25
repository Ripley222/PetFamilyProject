namespace PetFamily.Application.VolunteersFeatures.GetWithPagination;

public record GetVolunteersQuery(
    int Page,
    int PageSize);