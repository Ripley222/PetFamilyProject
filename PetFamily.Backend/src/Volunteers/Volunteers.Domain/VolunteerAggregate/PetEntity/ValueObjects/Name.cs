using CSharpFunctionalExtensions;
using SharedKernel;

namespace Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

public record Name
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.GeneralErrors.ValueIsRequired("PetName");
        
        if (value.Length > Constants.MAX_LENGTH_NAME)
            return Errors.GeneralErrors.ValueIsInvalid("PetName");

        return new Name(value);
    }
}