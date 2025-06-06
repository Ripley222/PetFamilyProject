using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.VolunterAggregate.PetEntity.ValueObjects;

namespace PetFamily.Domain.Entities.VolunterAggregate.PetEntity;

public class Pet : Entity<Guid>
{
    //ef core ctor
    private Pet(){}

    public Pet(
        Guid id, 
        string name, 
        SpeciesBreed speciesBreed, 
        string description, 
        string color, 
        string healthInformation, 
        Address address, 
        BodySize bodySize, 
        PhoneNumber phoneNumber,
        bool isNeutered, 
        DateOnly dateOfBirth, 
        bool isVaccinated, 
        HelpStatus helpStatus,
        Requisities requisities)
    {
        Id = id;
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
        Requisities = requisities;
        Created = DateOnly.FromDateTime(DateTime.Now);
    }

    public string Name { get; private set; } = string.Empty;
    public SpeciesBreed SpeciesBreed { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string Color { get; private set; } = string.Empty;
    public string HealthInformation { get; private set; } = string.Empty;
    public Address Address { get; private set; }
    public BodySize BodySize { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public bool IsNeutered { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public bool IsVaccinated { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public Requisities Requisities { get; private set; }
    public DateOnly Created { get; private set; }

    public static Result<Pet> Create(
        Guid id,
        string name,
        SpeciesBreed speciesBreed,
        string description,
        string color,
        string healthInformation,
        Address address,
        BodySize bodySize,
        PhoneNumber phoneNumber,
        bool isNeutered,
        DateOnly dateOfBirth,
        bool isVaccinated,
        HelpStatus helpStatus,
        Requisities requisities)
    {
        if (id == Guid.Empty)
            return Result.Failure<Pet>("Не задан индентификацонный номер животного!");

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Pet>("Необходимо указать имя питомца!");

        if (name.Length < 2 || name.Length > 100)
            return Result.Failure<Pet>("Имя питомца должно состоять из 2-100 символов!");

        if (speciesBreed is null)
            return Result.Failure<Pet>("Необходимо указать вид и породу животного!");

        if (address is null)
            return Result.Failure<Pet>("Необходмио указать адрес по которому находится животное!");

        if (bodySize is null)
            return Result.Failure<Pet>("Необходимо указать размеры тела животного!");

        if (phoneNumber is null)
            return Result.Failure<Pet>("Необходимо указать номер телефона владельца животного!");

        if (helpStatus is null)
            return Result.Failure<Pet>("Необходимо указать статус помощи животному!");

        if(requisities is null)
            return Result.Failure<Pet>("Необходимо указать реквизиты для помощи животному!");

        var result = new Pet(id, name, speciesBreed, description, color, healthInformation, address, 
            bodySize, phoneNumber, isNeutered, dateOfBirth, isVaccinated, helpStatus, requisities);

        return Result.Success(result);
    }
}