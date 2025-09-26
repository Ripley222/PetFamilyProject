using PetFamily.Domain.Entities.SpeciesAggregate;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;

namespace PetFamily.Application.Database;

public interface IReadDbContext
{
    IQueryable<Volunteer> VolunteersRead { get; }
    IQueryable<Pet> PetsRead { get; }
    IQueryable<Species> SpeciesRead { get; }
    IQueryable<Breed> BreedsRead { get; }
}