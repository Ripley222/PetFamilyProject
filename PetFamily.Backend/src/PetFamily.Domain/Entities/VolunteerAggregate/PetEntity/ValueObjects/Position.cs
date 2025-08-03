using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;

public record Position
{
    public static Position First => new(1);    
    public int Value { get; }
    
    private Position(int value)
    {
        Value = value;
    }

    public static Result<Position, Error> Create(int number)
    {
        if (number <= 0)
            return Errors.General.ValueIsInvalid("Position");
        
        return new Position(number);
    }
}