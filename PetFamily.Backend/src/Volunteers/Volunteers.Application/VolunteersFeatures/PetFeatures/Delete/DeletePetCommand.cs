using Application.Abstraction;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.Delete;

public record DeletePetCommand(
    Guid VolunteerId,
    Guid PetId) : ICommand;