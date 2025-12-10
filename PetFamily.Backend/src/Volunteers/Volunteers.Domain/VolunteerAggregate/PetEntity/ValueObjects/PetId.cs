namespace Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

public record PetId : IComparable<PetId>
{
    public Guid Value { get; }

    private PetId(Guid value)
    {
        Value = value;
    }

    public static PetId New() => new PetId(Guid.NewGuid());
    public static PetId Empty() => new PetId(Guid.Empty);
    public static PetId Create(Guid value) => new PetId(value);
    
    public int CompareTo(PetId? other)
    {
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}