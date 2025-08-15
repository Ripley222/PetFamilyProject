using PetFamily.Application.VolunteersFeatures.DTOs;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add;

public record AddPetFileCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<FileDto> Files);