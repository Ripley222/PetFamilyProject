using Application.Abstraction;

namespace Volunteers.Application.VolunteersFeatures.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;