using Volunteers.Application.VolunteersFeatures.GetWithPagination;

namespace Volunteers.Presenters.Requests.Volunteer.Get;

public record GetVolunteersRequest(int Page = 1, int PageSize = 10)
{
    public GetVolunteersQuery ToQuery() => new GetVolunteersQuery(Page, PageSize);
}