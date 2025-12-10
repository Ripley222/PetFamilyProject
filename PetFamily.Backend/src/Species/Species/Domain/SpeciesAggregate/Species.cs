using CSharpFunctionalExtensions;
using SharedKernel;
using Species.Domain.SpeciesAggregate.ValueObjects;

namespace Species.Domain.SpeciesAggregate;

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

    public string Name { get; private set; } = null!;
    
    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Result<Species, Error> Create(SpeciesId speciesId, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Failure("specis.create", "Необходимо указать название вида!");

        if (name.Length > Constants.MAX_LENGTH_NAME)
            return Error.Failure("species.create", $"Длина названия больше {Constants.MAX_LENGTH_NAME} символов!");

        var species = new Species(speciesId, name);

        return species;
    }

    public UnitResult<Error> AddBreed(Breed breed)
    {
        if (_breeds.Contains(breed))
            return Errors.BreedsErrors.AlreadyExist();

        _breeds.Add(breed);

        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> RemoveBreed(Breed breed)
    {
        _breeds.Remove(breed);

        return UnitResult.Success<Error>();
    }
}