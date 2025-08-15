using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SpeciesAggregate;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.SpeciesFeatures;

public interface ISpeciesRepository
{
    Task<Result<Species, Error>> GetByName(string name, CancellationToken cancellationToken = default);
}