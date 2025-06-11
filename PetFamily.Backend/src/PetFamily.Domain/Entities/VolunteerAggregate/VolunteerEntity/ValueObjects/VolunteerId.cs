namespace PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;

public record VolunteerId : IComparable<VolunteerId>
{
    public Guid Value { get; }

    private VolunteerId(Guid value)
    {
        Value = value;
    }
    
    public static VolunteerId New() => new VolunteerId(Guid.NewGuid());
    public static VolunteerId Empty() => new VolunteerId(Guid.Empty);
    public static VolunteerId Create(Guid value) => new VolunteerId(value);
    
    public int CompareTo(VolunteerId? other)
    {
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}