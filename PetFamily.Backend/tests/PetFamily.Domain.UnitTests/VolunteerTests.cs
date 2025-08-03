using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace PetFamily.Domain.UnitTests;

public class VolunteerTests
{
    [Fact]
    public void Add_First_Pet_ReturnResult()
    {
        // arrange
        var volunteerId = VolunteerId.New();
        var fullName = FullName.Create("TestFirstName", "TestMiddleName", "TestLastName").Value;
        var emailAddress = EmailAddress.Create("TestEmailAddress@gmail.com").Value;
        var description = Description.Create("TestDescription").Value;
        var yearsOfExperience = 0;
        var phoneNumber = PhoneNumber.Create("+77777777777").Value;
        var requisites = new List<Requisite>();
        var socialNetworks = new List<SocialNetwork>();

        var volunteer = Volunteer.Create(
            volunteerId,
            fullName,
            emailAddress,
            description,
            yearsOfExperience,
            phoneNumber,
            requisites,
            socialNetworks).Value;

        var petId = PetId.New();
        var name = Name.Create("TestName").Value;
        var speciesBreed = SpeciesBreed.Create(SpeciesId.New(), BreedId.New()).Value;
        var color = "TestColor";
        var healthInformation = HealthInformation.Create("TestHealthInformation").Value;
        var address = Address.Create("TestAddress", "TestStreet", "TestHouse").Value;
        var bodySize = BodySize.Create(BodySize.MIN_WEIGHT, BodySize.MIN_HEIGHT).Value;
        var isNeutured = true;
        var dateOfBirth = DateOnly.MinValue;
        var isVactinated = true;
        var helpStatus = HelpStatus.LookingHome;
        
        var pet = Pet.Create(
            petId, 
            name,
            speciesBreed,
            description,
            color,
            healthInformation,
            address,
            bodySize,
            phoneNumber,
            isNeutured,
            dateOfBirth,
            isVactinated,
            helpStatus,
            requisites).Value;
        
        // act
        var result = volunteer.AddPet(pet);

        // assert
        var addedPet = volunteer.GetPetById(petId);
        
        Assert.True(result.IsSuccess);
        Assert.True(addedPet.IsSuccess);
        Assert.Equal(addedPet.Value.Id, pet.Id);
        Assert.Equal(addedPet.Value.Position, Position.First);
    }
    
    [Fact]
    public void Add_Some_Pet_ReturnResult()
    {
        // arrange
        var fullName = FullName.Create("TestFirstName", "TestMiddleName", "TestLastName").Value;
        var emailAddress = EmailAddress.Create("TestEmailAddress@gmail.com").Value;
        var description = Description.Create("TestDescription").Value;
        var yearsOfExperience = 0;
        var phoneNumber = PhoneNumber.Create("+77777777777").Value;
        var requisites = new List<Requisite>();
        var socialNetworks = new List<SocialNetwork>();

        var volunteer = Volunteer.Create(
            VolunteerId.New(),
            fullName,
            emailAddress,
            description,
            yearsOfExperience,
            phoneNumber,
            requisites,
            socialNetworks).Value;
    
        var firstPetId = PetId.New();
        var nameFirstPet = Name.Create("TestFirstName").Value;
        var speciesBreed = SpeciesBreed.Create(SpeciesId.New(), BreedId.New()).Value;
        var color = "TestColor";
        var healthInformation = HealthInformation.Create("TestHealthInformation").Value;
        var address = Address.Create("TestCity", "TestStreet", "TestHouse").Value;
        var bodySize = BodySize.Create(BodySize.MIN_WEIGHT, BodySize.MIN_HEIGHT).Value;
        var isNeutered = true;
        var dateOfBirth = DateOnly.MinValue;
        var isVaccinated = true;
        var helpStatus = HelpStatus.LookingHome;
        var firstPet = Pet.Create(
            firstPetId, 
            nameFirstPet,
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
            requisites).Value;
        
        var secondPetId = PetId.New();
        var nameSecondPet = Name.Create("TestSecondName").Value;
        var secondPet = Pet.Create(
            secondPetId, 
            nameSecondPet,
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
            requisites).Value;
        
        var thirdPetId = PetId.New();
        var nameThirdPet = Name.Create("TestThirdName").Value;
        var thirdPet = Pet.Create(
            thirdPetId, 
            nameThirdPet,
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
            requisites).Value;
        
        // act
        var addedFirstPetResult = volunteer.AddPet(firstPet);
        var addedSecondPetResult = volunteer.AddPet(secondPet);
        var addedThirdPetResult = volunteer.AddPet(thirdPet);

        // assert
        var firstPosition = Position.First;
        var secondPosition = Position.Create(2).Value;
        var thirdPosition = Position.Create(3).Value;
        
        var firstAddedPet = volunteer.GetPetById(firstPetId).Value;
        var secondAddedPet = volunteer.GetPetById(secondPetId).Value;
        var thirdAddedPet = volunteer.GetPetById(thirdPetId).Value;
        
        Assert.True(addedFirstPetResult.IsSuccess);
        Assert.True(addedSecondPetResult.IsSuccess);
        Assert.True(addedThirdPetResult.IsSuccess);
        
        Assert.Equal(firstAddedPet.Id.Value, firstPetId.Value);
        Assert.Equal(secondAddedPet.Id.Value, secondPetId.Value);
        Assert.Equal(thirdAddedPet.Id.Value, thirdPetId.Value);
        
        Assert.Equal(firstAddedPet.Position.Value, firstPosition.Value);
        Assert.Equal(secondAddedPet.Position.Value, secondPosition.Value);
        Assert.Equal(thirdAddedPet.Position.Value, thirdPosition.Value);
    }
    
    [Fact]
    public void Move_First_Pet_To_The_Third_Position_ReturnResult()
    {
        // arrange
        var fullName = FullName.Create("TestFirstName", "TestMiddleName", "TestLastName").Value;
        var emailAddress = EmailAddress.Create("TestEmailAddress@gmail.com").Value;
        var description = Description.Create("TestDescription").Value;
        var yearsOfExperience = 0;
        var phoneNumber = PhoneNumber.Create("+77777777777").Value;
        var requisites = new List<Requisite>();
        var socialNetworks = new List<SocialNetwork>();
        var volunteer = Volunteer.Create(
            VolunteerId.New(),
            fullName,
            emailAddress,
            description,
            yearsOfExperience,
            phoneNumber,
            requisites,
            socialNetworks).Value;
    
        var firstPetId = PetId.New();
        var nameFirstPet = Name.Create("TestFirstName").Value;
        var speciesBreed = SpeciesBreed.Create(SpeciesId.New(), BreedId.New()).Value;
        var color = "TestColor";
        var healthInformation = HealthInformation.Create("TestHealthInformation").Value;
        var address = Address.Create("TestCity", "TestStreet", "TestHouse").Value;
        var bodySize = BodySize.Create(BodySize.MIN_WEIGHT, BodySize.MIN_HEIGHT).Value;
        var isNeutered = true;
        var dateOfBirth = DateOnly.MinValue;
        var isVaccinated = true;
        var helpStatus = HelpStatus.LookingHome;
        var firstPet = Pet.Create(
            firstPetId, 
            nameFirstPet,
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
            requisites).Value;
        
        var secondPetId = PetId.New();
        var nameSecondPet = Name.Create("TestSecondName").Value;
        var secondPet = Pet.Create(
            secondPetId, 
            nameSecondPet,
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
            requisites).Value;
        
        var thirdPetId = PetId.New();
        var nameThirdPet = Name.Create("TestThirdName").Value;
        var thirdPet = Pet.Create(
            thirdPetId, 
            nameThirdPet,
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
            requisites).Value;
        
        volunteer.AddPet(firstPet);
        volunteer.AddPet(secondPet);
        volunteer.AddPet(thirdPet);
        
        var firstPosition = Position.First;
        var secondPosition = Position.Create(2).Value;
        var thirdPosition = Position.Create(3).Value;
        
        // act
        var result = volunteer.MovePet(firstPet, thirdPosition);
        
        // assert
        var firstAddedPet = volunteer.GetPetById(firstPetId).Value;
        var secondAddedPet = volunteer.GetPetById(secondPetId).Value;
        var thirdAddedPet = volunteer.GetPetById(thirdPetId).Value;
        
        Assert.True(result.IsSuccess);
        
        Assert.Equal(firstAddedPet.Id.Value, firstPetId.Value);
        Assert.Equal(secondAddedPet.Id.Value, secondPetId.Value);
        Assert.Equal(thirdAddedPet.Id.Value, thirdPetId.Value);
        
        Assert.Equal(firstAddedPet.Position.Value, thirdPosition.Value);
        Assert.Equal(secondAddedPet.Position.Value, firstPosition.Value);
        Assert.Equal(thirdAddedPet.Position.Value, secondPosition.Value);
    }
}
