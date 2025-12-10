namespace Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects
{
    public record SpeciesBreed
    {
        public Guid SpeciesId { get; }
        public Guid BreedId { get; }

        public SpeciesBreed(Guid speciesId, Guid breedId)
        {
            SpeciesId = speciesId;
            BreedId = breedId;
        }
    }
}
