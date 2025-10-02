namespace PetFamily.Application.VolunteersFeatures.PetFeatures.Delete;

public record DeletePetCommand(
    Guid VolunteerId,
    Guid PetId);