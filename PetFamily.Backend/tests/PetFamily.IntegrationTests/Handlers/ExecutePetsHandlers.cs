using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Add;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Delete.HardDelete;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Delete.SoftDelete;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Move;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add.AddMainFile;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Add.AddManyFiles;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.Delete;
using PetFamily.Application.VolunteersFeatures.PetFeatures.PetFiles.GetLink;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Update.FullInfo;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Update.Status;
using PetFamily.Domain.Entities.SpeciesAggregate;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Handlers;

public class ExecutePetsHandlers(WebTestsFactory testsFactory) : BaseForTests(testsFactory)
{
    protected async Task<T> ExecuteCreateHandlers<T>(Func<AddPetHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<AddPetHandler>();
        
        return await action(sut);
    }

    protected async Task<T> ExecuteMoveHandlers<T>(Func<MovePetHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<MovePetHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteHardDeleteHandlers<T>(Func<HardDeletePetHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<HardDeletePetHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteSofDeleteHandlers<T>(Func<SoftDeletePetHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<SoftDeletePetHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteAddMainFileHandlers<T>(Func<AddMainFileHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<AddMainFileHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteAddFilesHandlers<T>(Func<AddPetFilesHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<AddPetFilesHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteRemoveFileHandlers<T>(Func<DeletePetFileHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<DeletePetFileHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteGetFileLinkHandlers<T>(Func<GetPetFileLinkHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<GetPetFileLinkHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteUpdateFullInfoHandlers<T>(Func<UpdatePetHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<UpdatePetHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteUpdateHelpStatusHandlers<T>(Func<UpdatePetStatusHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<UpdatePetStatusHandler>();
        
        return await action(sut);
    }

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
        string petName, CancellationToken cancellationToken)
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
}