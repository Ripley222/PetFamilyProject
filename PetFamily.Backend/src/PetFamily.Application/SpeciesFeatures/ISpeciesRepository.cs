using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SpeciesAggregate;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.SpeciesFeatures;

public interface ISpeciesRepository
{
    Task<Result<Guid, Error>> RemoveSpecies(Species species, CancellationToken cancellationToken = default);

    Task<Result<Guid, Error>> RemoveBreed(Breed breed, CancellationToken cancellationToken = default);
}