using Volunteers.Application.VolunteersFeatures.DTOs;
using Volunteers.Application.VolunteersFeatures.Update.Requisites;

namespace Volunteers.Presenters.Requests.Volunteer.Update;

public record UpdateVolunteerRequisitesRequest(
    IEnumerable<RequisitesDto> Requisites)
{
    public UpdateRequisitesCommand ToCommand(Guid volunteerId) =>
        new(volunteerId,
            Requisites);
}