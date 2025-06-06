using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;

namespace PetFamily.Domain.Entities.SpeciesAggregate;

public class Breed
{
    //ef core ctor
    private Breed() { }

    private Breed(BreedId id, string name)
    {
        Id = id;
        Name = name;
    }

    public BreedId Id { get; private set; }
    public string Name { get; private set; }

    public static Result<Breed> Create(BreedId id, string name)
    {
        if (id is null)
            return Result.Failure<Breed>("Необходимо указать идентификационный номер породы!");

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Breed>("Необходимо указать название породы!");

        var result = new Breed(id, name);

        return Result.Success(result);
    }
}