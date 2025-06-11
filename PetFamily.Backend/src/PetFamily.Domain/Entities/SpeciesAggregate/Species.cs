using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.SpeciesAggregate;

public class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    
    //ef core ctor
    private Species(SpeciesId id) : base(id)
    {
    }

    private Species(SpeciesId id, string name) :  base(id)
    {
        Name = name;
    }
    
    public string Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Result<Species> Create(SpeciesId speciesId, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Species>("Необходимо указать название вида!");
        
        if (name.Length < Constants.MIN_LENGTH_NAME || name.Length > Constants.MAX_LENGTH_NAME)
            return Result.Failure<Species>($"Длина названия вида должна составлять {Constants.MIN_LENGTH_NAME}-{Constants.MAX_LENGTH_NAME} символов!");

        var result = new Species(speciesId, name);

        return Result.Success(result);
    }

    public Result AddBreed(Breed breed)
    {
        if (_breeds.Contains(breed))
            return Result.Failure("Данный вид уже содержит указанную породу!");

        _breeds.Add(breed);

        return Result.Success();
    }
}