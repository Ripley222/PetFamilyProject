using CSharpFunctionalExtensions;
using SharedKernel;
using Species.Domain.SpeciesAggregate.ValueObjects;

namespace Species.Domain.SpeciesAggregate;

public class Breed : Entity<BreedId>
{
    //ef core ctor
    private Breed(BreedId id) : base(id)
    {
    }

    private Breed(BreedId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; } = null!;

    public static Result<Breed, Error> Create(BreedId breedId, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Failure("breed.create", "Необходимо указать название породы!");

        if (name.Length > Constants.MAX_LENGTH_NAME)
            return Error.Failure("breed.create", $"Длина названия больше {Constants.MAX_LENGTH_NAME} символов!");

        var breed = new Breed(breedId, name);

        return breed;
    }
}