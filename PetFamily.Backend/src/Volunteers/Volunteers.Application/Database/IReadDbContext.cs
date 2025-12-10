using Volunteers.Domain.VolunteerAggregate.PetEntity;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity;

namespace Volunteers.Application.Database;

public interface IReadDbContext
{
    IQueryable<Volunteer> VolunteersRead { get; }
    IQueryable<Pet> PetsRead { get; }
}