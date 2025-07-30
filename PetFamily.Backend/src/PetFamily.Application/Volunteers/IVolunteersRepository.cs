using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default);
    
    Task<Result<Volunteer, Error>> GetByFullName(FullName fullName, CancellationToken cancellationToken = default);
    
    Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default);
}