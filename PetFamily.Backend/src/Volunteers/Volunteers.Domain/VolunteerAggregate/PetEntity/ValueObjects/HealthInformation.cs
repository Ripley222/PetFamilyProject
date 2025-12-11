using CSharpFunctionalExtensions;
using SharedKernel;

namespace Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

public record HealthInformation
{
    public const int MIN_VALUE_LENGTH = 5;
    public const int MAX_VALUE_LENGTH = 1000;
    
    public string Value { get; }

    private HealthInformation(string value)
    {
        Value = value;
    }

    public static Result<HealthInformation, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.GeneralErrors.ValueIsRequired("HealthInformation");
        
        if (value.Length < MIN_VALUE_LENGTH || value.Length > MAX_VALUE_LENGTH)
            return Errors.GeneralErrors.ValueIsInvalid("HealthInformation");

        return new HealthInformation(value);
    }
}