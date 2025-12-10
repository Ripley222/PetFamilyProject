using Application.Abstraction;

namespace Volunteers.Application.VolunteersFeatures.GetById;

public record GetVolunteersByIdQuery(Guid VolunteerId) : IQuery;