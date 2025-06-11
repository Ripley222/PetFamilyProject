using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;

public record Description
{
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Description>("Необходиом указать описание!");
        
        if (value.Length < Constants.MIN_LENGTH_DESCRIPTION || value.Length > Constants.MAX_LENGTH_DESCRIPTION)
            return Result.Failure<Description>($"Длина описания должна быть {Constants.MIN_LENGTH_DESCRIPTION}-{Constants.MAX_LENGTH_DESCRIPTION} символов!");

        var description = new Description(value);
        
        return Result.Success(description);
    }
}