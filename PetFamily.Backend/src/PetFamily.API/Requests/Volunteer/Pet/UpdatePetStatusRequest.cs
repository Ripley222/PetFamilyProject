using PetFamily.Application.VolunteersFeatures.PetFeatures.Update.Status;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.Enums;

namespace PetFamily.API.Requests.Volunteer.Pet;

public record UpdatePetStatusRequest(
    HelpStatusEnum HelpStatus)
{
    public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId) 
        => new UpdatePetStatusCommand(volunteerId, petId, HelpStatus.ToString());
}