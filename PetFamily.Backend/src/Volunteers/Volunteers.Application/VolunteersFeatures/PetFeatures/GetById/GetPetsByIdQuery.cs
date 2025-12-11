using Application.Abstraction;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.GetById;

public record GetPetsByIdQuery(Guid PetId) : IQuery;