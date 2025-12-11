using CSharpFunctionalExtensions;
using SharedKernel;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures;

public interface IVolunteersRepository
{
    Task<Result<Guid, Error>> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> Save(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> Delete(Volunteer volunteer, CancellationToken cancellationToken = default);
}