using Application.Abstraction;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.GetLink;

public record GetPetFileLinkCommand(
    Guid VolunteerId,
    Guid PetId,
    string FileName) : ICommand;