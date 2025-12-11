using Volunteers.Application.VolunteersFeatures.PetFeatures.Update.Status;
using Volunteers.Domain.VolunteerAggregate.PetEntity.Enums;

namespace Volunteers.Presenters.Requests.Volunteer.Pet;

public record UpdatePetStatusRequest(
    HelpStatusEnum HelpStatus)
{
    public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId) 
        => new UpdatePetStatusCommand(volunteerId, petId, HelpStatus.ToString());
}