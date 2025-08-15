namespace PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;

public record DeletePetFileCommand(
    Guid VolunteerId,
    Guid PetId,
    string FileName);