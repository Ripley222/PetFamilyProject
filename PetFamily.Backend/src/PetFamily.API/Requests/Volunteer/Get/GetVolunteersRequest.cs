using PetFamily.Application.VolunteersFeatures.GetWithPagination;

namespace PetFamily.API.Requests.Volunteer.Get;

public record GetVolunteersRequest(int Page = 1, int PageSize = 10)
{
    public GetVolunteersQuery ToQuery() => new GetVolunteersQuery(Page, PageSize);
}