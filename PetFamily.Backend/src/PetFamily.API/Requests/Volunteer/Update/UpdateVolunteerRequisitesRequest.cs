using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Application.VolunteersFeatures.Update.Requisites;

namespace PetFamily.API.Requests.Volunteer.Update;

public record UpdateVolunteerRequisitesRequest(
    IEnumerable<RequisitesDto> Requisites)
{
    public UpdateRequisitesCommand ToCommand(Guid volunteerId) =>
        new(volunteerId,
            Requisites);
}