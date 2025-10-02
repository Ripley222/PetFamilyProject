using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures;

public interface IVolunteersRepository
{
    Task<Result<Guid, Error>> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> Save(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> Delete(Volunteer volunteer, CancellationToken cancellationToken = default);
}