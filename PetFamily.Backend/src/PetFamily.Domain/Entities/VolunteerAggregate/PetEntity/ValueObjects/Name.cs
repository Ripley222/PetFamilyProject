using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;

public record Name
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Result.Failure<Name>("Необходимо указать имя питомца!");
        
        if (value.Length < Constants.MIN_LENGTH_NAME || value.Length > Constants.MAX_LENGTH_NAME)
            return Result.Failure<Name>("Имя питомца должно состоять из 2-100 символов!");

        var name = new Name(value);
        
        return Result.Success(name);
    }
}