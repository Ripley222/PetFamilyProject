using Application.Abstraction;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.Update.Status;

public record UpdatePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    string HelpStatus) : ICommand;