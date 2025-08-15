using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;

public record FilePath
{
    public string Value { get; }

    private FilePath(string value)
    {
        Value = value;
    }

    public static Result<FilePath, Error> Create(Guid path, string extension)
    {
        return new FilePath(path + extension);
    }
}