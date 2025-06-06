using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;

namespace PetFamily.Domain.Entities.SpeciesAggregate;

public class Species
{
    private readonly List<Breed> _breeds = [];
    
    //ef core ctor
    private Species() { }

    private Species(SpeciesId id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public SpeciesId Id { get; private set; }
    public string Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Result<Species> Create(SpeciesId speciesId, string name)
    {
        if (speciesId is null)
            return Result.Failure<Species>("Необходимо указать идентификационный номер вида!");

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Species>("Необходимо указать название вида!");

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