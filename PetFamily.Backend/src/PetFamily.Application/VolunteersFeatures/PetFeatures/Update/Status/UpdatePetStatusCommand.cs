namespace PetFamily.Application.VolunteersFeatures.PetFeatures.Update.Status;

public record UpdatePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    string HelpStatus);