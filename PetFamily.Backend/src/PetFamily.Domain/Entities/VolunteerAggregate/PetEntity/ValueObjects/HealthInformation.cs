using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;

public record HealthInformation
{
    public const int MIN_VALUE_LENGTH = 5;
    public const int MAX_VALUE_LENGTH = 1000;
    
    public string Value { get; }

    private HealthInformation(string value)
    {
        Value = value;
    }

    public static Result<HealthInformation> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<HealthInformation>("Необходиом указать информация о здоровье питомца!");
        
        if (value.Length < MIN_VALUE_LENGTH || value.Length > MAX_VALUE_LENGTH)
            return Result.Failure<HealthInformation>("Длина информации должна быть 5-1000 символов!");

        var healthInformation = new HealthInformation(value);
        
        return Result.Success(healthInformation);
    }
}