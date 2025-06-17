using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.SpeciesAggregate;

public class Breed : Entity<BreedId>
{
    //ef core ctor
    private Breed(BreedId id) : base(id)
    {
    }

    private Breed (BreedId id, string name) : base(id)
    {
        Name = name;
    }
    
    public string Name { get; private set; }

    public static Result<Breed> Create(BreedId breedId, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Breed>("Необходимо указать название породы!");
        
        if (name.Length > Constants.MAX_LENGTH_NAME)
            return Result.Failure<Breed>($"");

        var result = new Breed(breedId, name);

        return Result.Success(result);
    }
}