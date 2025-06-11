namespace PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects
{
    public record BreedId : IComparable<BreedId>
    {
        public Guid Value { get; }

        private BreedId(Guid value)
        {
            Value = value;
        }

        public static BreedId New() => new BreedId(Guid.NewGuid());
        public static BreedId Empty() => new BreedId(Guid.Empty);
        public static BreedId Create(Guid value) => new BreedId(value);
        
        public int CompareTo(BreedId? other)
        {
            if (other is null) return 1;
            return Value.CompareTo(other.Value);
        }
    }
}
