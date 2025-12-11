/*using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures.DTOs;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

namespace PetFamily.IntegrationTests.Pets;

public class CreateTests(WebTestsFactory webTestsFactory) : PetsEntityFactory(webTestsFactory)
{
    [Fact]
    public async Task CreatePet_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteerForPet = await CreateVolunteer(cancellationToken);
        var speciesBreedForPet = await CreateSpeciesBreed(cancellationToken);

        var name = "test";
        var description = "test";
        var color = "test";
        var healthInformation = "test health information";
        var city = "test";
        var street = "test";
        var house = "test";
        var weigth = 1.5;
        var height = 1.5;
        var phoneNumber = "+79788339045";
        var isNeutered = true;
        var dateOfBirth = DateOnly.MaxValue;
        var isVaccinated = true;
        var helpStatus = nameof(HelpStatus.NeedsHelp);

        var requisites = new List<RequisitesDto>()
        {
            new()
            {
                AccountNumber = "00000000000000000000",
                Title = "test",
                Description = "test",
            }
        };

        //act
        var newPetIdResult = await ExecuteHandlers<AddPetHandler, Result<Guid, ErrorList>>(
            async sut =>
        {
            var command = new AddPetCommand(
                volunteerForPet.Id.Value,
                speciesBreedForPet.SpeciesId.Value,
                speciesBreedForPet.BreedId.Value,
                name,
                description,
                color,
                healthInformation,
                city,
                street,
                house,
                weigth,
                height,
                phoneNumber,
                isNeutered,
                dateOfBirth,
                isVaccinated,
                helpStatus,
                requisites);

            return await sut.Handle(command, cancellationToken);
        });

        //assert
        var petExists = await ExecuteInDatabase(async dbContext =>
        {
            var volunteer = await dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteerForPet.Id, cancellationToken);

            return volunteer?.Pets[0];
        });

        Assert.NotNull(petExists);
        Assert.Equal(newPetIdResult.Value, petExists.Id.Value);
    }
    
    [Fact]
    public async Task CreatePet_WithDuplicate_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteerForPet = await CreateVolunteer(cancellationToken);
        var speciesBreedForPet = await CreateSpeciesBreed(cancellationToken);

        var name = "test";
        var description = "test";
        var color = "test";
        var healthInformation = "test health information";
        var city = "test";
        var street = "test";
        var house = "test";
        var weigth = 1.5;
        var height = 1.5;
        var phoneNumber = "+79788339045";
        var isNeutered = true;
        var dateOfBirth = DateOnly.MaxValue;
        var isVaccinated = true;
        var helpStatus = nameof(HelpStatus.NeedsHelp);

        var requisites = new List<RequisitesDto>()
        {
            new()
            {
                AccountNumber = "00000000000000000000",
                Title = "test",
                Description = "test",
            }
        };
        
        var command = new AddPetCommand(
            volunteerForPet.Id.Value,
            speciesBreedForPet.SpeciesId.Value,
            speciesBreedForPet.BreedId.Value,
            name,
            description,
            color,
            healthInformation,
            city,
            street,
            house,
            weigth,
            height,
            phoneNumber,
            isNeutered,
            dateOfBirth,
            isVaccinated,
            helpStatus,
            requisites);

        //act
        var petIdResult = await ExecuteHandlers<AddPetHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));
        
        var duplicatePetIdResult = await ExecuteHandlers<AddPetHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        var firstPetExists = await ExecuteInDatabase(async dbContext =>
        {
            var volunteer = await dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteerForPet.Id, cancellationToken);

            return volunteer?.Pets[0];
        });

        Assert.NotNull(firstPetExists);
        Assert.Equal(petIdResult.Value, firstPetExists.Id.Value);
        
        Assert.True(duplicatePetIdResult.IsFailure);
        Assert.NotEmpty(duplicatePetIdResult.Error);
    }

    [Fact]
    public async Task CreatePet_WithInvalidData_ShouldFailure()
    {
        var cancellationToken = CancellationToken.None;
        
        var volunteerForPet = await CreateVolunteer(cancellationToken);
        var speciesBreedForPet = await CreateSpeciesBreed(cancellationToken);
        
        var name = "";
        var description = "";
        var color = "";
        var healthInformation = "";
        var city = "";
        var street = "";
        var house = "";
        var weigth = 0;
        var height = 0;
        var phoneNumber = "";
        var isNeutered = true;
        var dateOfBirth = DateOnly.MaxValue;
        var isVaccinated = true;
        var helpStatus = "";

        var requisites = new List<RequisitesDto>()
        {
            new()
            {
                AccountNumber = "",
                Title = "test",
                Description = "test",
            }
        };
        
        var command = new AddPetCommand(
            volunteerForPet.Id.Value,
            speciesBreedForPet.SpeciesId.Value,
            speciesBreedForPet.BreedId.Value,
            name,
            description,
            color,
            healthInformation,
            city,
            street,
            house,
            weigth,
            height,
            phoneNumber,
            isNeutered,
            dateOfBirth,
            isVaccinated,
            helpStatus,
            requisites);

        //act
        var petIdResult = await ExecuteHandlers<AddPetHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(petIdResult.IsFailure);
        Assert.NotEmpty(petIdResult.Error);
    }
}*/