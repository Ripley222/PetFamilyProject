using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;

namespace PetFamily.Domain.Entities.VolunterAggregate.PetEntity.ValueObjects
{
    public class SpeciesBreed : ValueObject
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return SpeciesId;
            yield return BreedId;
        }
    }
}
