using CSharpFunctionalExtensions;
using SharedKernel;

namespace Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

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
            return Errors.GeneralErrors.ValueIsRequired("Path");
        
        return new MainFile(path);
    }
}