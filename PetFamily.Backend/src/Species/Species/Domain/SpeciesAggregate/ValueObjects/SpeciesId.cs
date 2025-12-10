namespace Species.Domain.SpeciesAggregate.ValueObjects
{
    public record SpeciesId : IComparable<SpeciesId>
    {
        public Guid Value { get; }

        private SpeciesId(Guid value)
        {
            Value = value;
        }

        public static SpeciesId New() => new SpeciesId(Guid.NewGuid());
        public static SpeciesId Empty() => new SpeciesId(Guid.Empty);
        public static SpeciesId Create(Guid value) => new SpeciesId(value);
        
        public int CompareTo(SpeciesId? other)
        {
            if (other is null) return 1;
            return Value.CompareTo(other.Value);
        }
    }
}
