using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.SpeciesFeatures;
using PetFamily.Domain.Entities.SpeciesAggregate;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository(
    ApplicationDbContext dbContext,
    ILogger<SpeciesRepository> logger) : ISpeciesRepository
{
    public async Task<Result<Guid, Error>> RemoveSpecies(Species species, CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.Remove(species);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            return species.Id.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<Guid, Error>(Error.Failure("error.remove.species", ex.Message));
        }
    }

    public async Task<Result<Guid, Error>> RemoveBreed(Breed breed, CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.Remove(breed);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            return breed.Id.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<Guid, Error>(Error.Failure("error.remove.breed", ex.Message));
        }
    }
}