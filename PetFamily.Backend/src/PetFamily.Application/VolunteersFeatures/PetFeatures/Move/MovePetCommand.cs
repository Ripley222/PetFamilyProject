namespace PetFamily.Application.VolunteersFeatures.PetFeatures.Move;

public record MovePetCommand(
    Guid VolunteerId,
    Guid PetId,
    int NewPosition);