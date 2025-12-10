using Application.Abstraction;

namespace Volunteers.Application.VolunteersFeatures.GetWithPagination;

public record GetVolunteersQuery(
    int Page,
    int PageSize) : IQuery;