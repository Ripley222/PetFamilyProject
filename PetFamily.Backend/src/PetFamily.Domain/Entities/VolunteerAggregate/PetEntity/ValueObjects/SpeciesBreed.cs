using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects
{
    public record SpeciesBreed
    {
        public SpeciesId SpeciesId { get; }
        public BreedId BreedId { get; }

        private SpeciesBreed(SpeciesId speciesId, BreedId breedId)
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }

        public static Result<SpeciesBreed> Create(SpeciesId speciesId, BreedId breedId)
        {
            var result = new SpeciesBreed(speciesId, breedId);

            return Result.Success(result);
        }
    }
}
