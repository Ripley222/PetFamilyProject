using CSharpFunctionalExtensions;
using SharedKernel;

namespace Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

public record Description
{
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.GeneralErrors.ValueIsRequired("Description");
        
        if (value.Length > Constants.MAX_LENGTH_DESCRIPTION)
            return Errors.GeneralErrors.ValueIsInvalid("Description");

        return new Description(value);
    }
}