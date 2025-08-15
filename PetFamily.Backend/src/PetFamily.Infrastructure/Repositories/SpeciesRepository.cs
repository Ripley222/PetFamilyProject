using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.SpeciesFeatures;
using PetFamily.Domain.Entities.SpeciesAggregate;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository(ApplicationDbContext context) : ISpeciesRepository
{
    public async Task<Result<Species, Error>> GetByName(string name, CancellationToken cancellationToken = default)
    {
        var species = await context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);

        if (species == null)
            return Errors.Species.NotFound();

        return species;
    }
}