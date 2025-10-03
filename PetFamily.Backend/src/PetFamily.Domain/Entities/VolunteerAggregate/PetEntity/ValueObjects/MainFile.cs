using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;

public record MainFile
{
    public string Value { get; }

    private MainFile(string value)
    {
        Value = value;
    }

    public static Result<MainFile, Error> Create(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.General.ValueIsRequired("Path");
        
        return new MainFile(path);
    }
}