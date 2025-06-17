using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity;

public class Pet : Entity<PetId>
{
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
        List<Requisite> requisites) :  base(id)
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

    public Name Name { get; private set; }
    public SpeciesBreed SpeciesBreed { get; private set; }
    public Description Description { get; private set; }
    public string Color { get; private set; }
    public HealthInformation HealthInformation { get; private set; }
    public Address Address { get; private set; }
    public BodySize BodySize { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public bool IsNeutered { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public bool IsVaccinated { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public List<Requisite> Requisites { get; private set; }
    public DateOnly Created { get; private set; }

    public static Result<Pet> Create(
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
            return Result.Failure<Pet>("Необходимо указать цвет питомца!");
        
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

        return Result.Success(pet);
    }
}