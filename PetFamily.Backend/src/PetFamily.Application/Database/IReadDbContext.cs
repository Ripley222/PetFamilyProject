using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;

namespace PetFamily.Application.Database;

public interface IReadDbContext
{
    IQueryable<Volunteer> VolunteersRead { get; }
}