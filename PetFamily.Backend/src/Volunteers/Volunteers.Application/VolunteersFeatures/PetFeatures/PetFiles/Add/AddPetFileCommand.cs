using Application.Abstraction;
using Volunteers.Application.VolunteersFeatures.DTOs;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.Add;

public record AddPetFileCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<FileDto> Files) : ICommand;