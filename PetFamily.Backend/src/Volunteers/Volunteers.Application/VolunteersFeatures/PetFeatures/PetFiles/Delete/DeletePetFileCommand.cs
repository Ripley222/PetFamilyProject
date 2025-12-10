using Application.Abstraction;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;

public record DeletePetFileCommand(
    Guid VolunteerId,
    Guid PetId,
    string FileName) : ICommand;