using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity;

public class Pet : Entity<PetId>, IEquatable<Pet>
{
    private List<FilePath> _files = [];

    private bool _isDeleted = false;

    //ef core ctor
    private Pet(PetId id) : base(id)
    {
    }

    private Pet(
        PetId id,
        Name name,
        SpeciesBreed speciesBreed,
        Description description,
        string color,
        HealthInformation healthInformation,
        Address address,
        BodySize bodySize,
        PhoneNumber phoneNumber,
        bool isNeutered,
        DateOnly dateOfBirth,
        bool isVaccinated,
        HelpStatus helpStatus,
        List<Requisite> requisites) : base(id)
    {
        Name = name;
        SpeciesBreed = speciesBreed;
        Description = description;
        Color = color;
        HealthInformation = healthInformation;
        Address = address;
        BodySize = bodySize;
        PhoneNumber = phoneNumber;
        IsNeutered = isNeutered;
        DateOfBirth = dateOfBirth;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        Requisites = requisites;
        Created = DateOnly.FromDateTime(DateTime.Now);
    }

    public Name Name { get; private set; } = null!;
    public SpeciesBreed SpeciesBreed { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public string Color { get; private set; } = null!;
    public HealthInformation HealthInformation { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public BodySize BodySize { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public bool IsNeutered { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public bool IsVaccinated { get; private set; }
    public HelpStatus HelpStatus { get; private set; } = null!;
    public List<Requisite> Requisites { get; private set; } = null!;
    public DateOnly Created { get; private set; }
    public Position Position { get; private set; } = null!;
    public MainFile MainFile { get; private set; }
    public IReadOnlyList<FilePath> Files => _files;
    
    public VolunteerId VolunteerId { get; private set; } = null!;

    public static Result<Pet, Error> Create(
        PetId petId,
        Name name,
        SpeciesBreed speciesBreed,
        Description description,
        string color,
        HealthInformation healthInformation,
        Address address,
        BodySize bodySize,
        PhoneNumber phoneNumber,
        bool isNeutered,
        DateOnly dateOfBirth,
        bool isVaccinated,
        HelpStatus helpStatus,
        List<Requisite> requisites)
    {
        if (string.IsNullOrEmpty(color))
            return Error.Failure("pet.create", "Failed to create pet");

        var pet = new Pet(
            petId,
            name,
            speciesBreed,
            description,
            color,
            healthInformation,
            address,
            bodySize,
            phoneNumber,
            isNeutered,
            dateOfBirth,
            isVaccinated,
            helpStatus,
            requisites);

        return pet;
    }

    public UnitResult<Error> Update(
        Name name,
        SpeciesBreed speciesBreed,
        Description description,
        string color,
        HealthInformation healthInformation,
        Address address,
        BodySize bodySize,
        PhoneNumber phoneNumber,
        bool isNeutered,
        DateOnly dateOfBirth,
        bool isVaccinated,
        HelpStatus helpStatus,
        List<Requisite> requisites)
    {
        if (string.IsNullOrEmpty(color))
            return Error.Failure("pet.update", "Failed to updated pet");

        Name = name;
        SpeciesBreed = speciesBreed;
        Description = description;
        Color = color;
        HealthInformation = healthInformation;
        Address = address;
        BodySize = bodySize;
        PhoneNumber = phoneNumber;
        IsNeutered = isNeutered;
        DateOfBirth = dateOfBirth;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        Requisites = requisites;

        return UnitResult.Success<Error>();
    }

    public void UpdateStatus(HelpStatus helpStatus)
    {
        HelpStatus = helpStatus;
    }

    public void AddPhoto(FilePath filePath)
    {
        _files.Add(filePath);
    }

    public void SetMainPhoto(MainFile mainFile)
    {
        MainFile = mainFile;
    }

    public void DeletePhoto(FilePath filePath)
    {
        _files.Remove(filePath);
    }

    public void SetPosition(Position position)
    {
        Position = position;
    }

    public void Delete()
    {
        _isDeleted = true;
    }

    public void Restore()
    {
        _isDeleted = false;
    }

    public bool Equals(Pet? other)
    {
        if (other == null) return false;

        return Name == other.Name &&
               SpeciesBreed == other.SpeciesBreed &&
               Description == other.Description &&
               Color == other.Color &&
               HealthInformation == other.HealthInformation &&
               Address == other.Address &&
               BodySize == other.BodySize &&
               PhoneNumber == other.PhoneNumber &&
               IsNeutered == other.IsNeutered &&
               DateOfBirth == other.DateOfBirth &&
               IsVaccinated == other.IsVaccinated &&
               HelpStatus == other.HelpStatus &&
               Requisites.SequenceEqual(other.Requisites);
    }

    public override bool Equals(object? obj) => Equals(obj as Pet);

    public override int GetHashCode() => Id.GetHashCode();
}