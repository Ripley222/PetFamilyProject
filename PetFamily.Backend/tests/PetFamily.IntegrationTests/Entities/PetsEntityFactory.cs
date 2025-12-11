/*using PetFamily.IntegrationTests.Infrastructure;
using Volunteers.Domain.VolunteerAggregate.PetEntity;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace PetFamily.IntegrationTests.Entities;

public class PetsEntityFactory(WebTestsFactory testsFactory) : BaseForTests(testsFactory)
{
    protected async Task<Volunteer> CreateVolunteer(CancellationToken cancellationToken = default)
    {
        return await ExecuteInDatabase(async dbContext =>
        {
            var volunteerForPet = Volunteer.Create(
                VolunteerId.New(),
                FullName.Create("test", "test", "test").Value,
                EmailAddress.Create("test@gmail.com").Value,
                Description.Create("test").Value,
                1,
                PhoneNumber.Create("+79788339045").Value,
                new List<Requisite>
                {
                    Requisite.Create("00000000000000000000", "test", "test").Value
                },
                new List<SocialNetwork>
                {
                    SocialNetwork.Create("test", "http://www.test.com").Value
                });

            var addedVolunteer = await dbContext.Volunteers.AddAsync(volunteerForPet.Value, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return addedVolunteer.Entity;
        });
    }

    protected async Task<Volunteer> CreateVolunteer(
        IEnumerable<Pet> pets, CancellationToken cancellationToken = default)
    {
        return await ExecuteInDatabase(async dbContext =>
        {
            var volunteerForPet = Volunteer.Create(
                VolunteerId.New(),
                FullName.Create("test", "test", "test").Value,
                EmailAddress.Create("test@gmail.com").Value,
                Description.Create("test").Value,
                1,
                PhoneNumber.Create("+79788339045").Value,
                new List<Requisite>
                {
                    Requisite.Create("00000000000000000000", "test", "test").Value
                },
                new List<SocialNetwork>
                {
                    SocialNetwork.Create("test", "http://www.test.com").Value
                });

            foreach (var pet in pets)
            {
                volunteerForPet.Value.AddPet(pet);
            }
            
            var addedVolunteer = await dbContext.Volunteers.AddAsync(volunteerForPet.Value, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return addedVolunteer.Entity;
        });
    }
    
    protected async Task<Pet> CreatePet(
        string petName, CancellationToken cancellationToken = default)
    {
        var speciesBreed = await CreateSpeciesBreed(cancellationToken);
        
        var pet = Pet.Create(
            PetId.New(),
            Name.Create(petName).Value,
            SpeciesBreed.Create(speciesBreed.SpeciesId, speciesBreed.BreedId).Value,
            Description.Create("test").Value,
            "test",
            HealthInformation.Create("test-health-information").Value,
            Address.Create("city", "street", "house").Value,
            BodySize.Create(1.5, 1.5).Value,
            PhoneNumber.Create("+79788339045").Value,
            true,
            DateOnly.MaxValue,
            true,
            HelpStatus.FoundHome, 
            new List<Requisite>
            {
                Requisite.Create("00000000000000000000", "test", "test").Value
            });

        return pet.Value;
    }
    
    protected async Task<SpeciesBreed> CreateSpeciesBreed(CancellationToken cancellationToken = default)
    {
        return await ExecuteInDatabase(async dbContext =>
        {
            var species = Species.Create(SpeciesId.New(), "test").Value;
            var breed = Breed.Create(BreedId.New(), "test").Value;

            species.AddBreed(breed);
            
            await dbContext.Species.AddAsync(species, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var speciesBreed = SpeciesBreed.Create(species.Id, breed.Id).Value;
            
            return speciesBreed;
        });
    }
}*/