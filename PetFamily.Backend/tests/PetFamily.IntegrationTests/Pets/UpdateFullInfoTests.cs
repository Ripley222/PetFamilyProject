using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Update.FullInfo;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Pets;

public class UpdateFullInfoTests(WebTestsFactory testsFactory) : PetsEntityFactory(testsFactory)
{
    [Fact]
    public async Task UpdateFullInfo_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var pet = await CreatePet("test-pet", cancellationToken);
        var volunteer = await CreateVolunteer([pet], cancellationToken);

        var updatedSpeciesBreed = await CreateSpeciesBreed(cancellationToken);
        var updatedName = "test-pet-updated";
        var updatedDescription = "test-description-updated";
        var updatedColor = "test-color-updated";
        var updatedHealthInformation = "test-health-info-updated";
        var updatedCity = "test-city-updated";
        var updatedStreet = "test-street-updated";
        var updatedHouse = "100/100";
        var updatedWeight = 2.5;
        var updatedHeight = 2.5;
        var updatedPhoneNumber = "+79788339090";
        var updatedIsNeutered = false;
        DateOnly updatedDateOnBirth = DateOnly.FromDateTime(DateTime.Now);
        var updatedIsVaccinated = false;
        var updatedHelpStatus = HelpStatus.LookingHome.Value;
        var updatedRequisites = new List<RequisitesDto>
        {
            new()
            {
                AccountNumber = "11111111111111111111",
                Description = "updated-description",
                Title = "updated-title"
            }
        };

        var command = new UpdatePetCommand(
            volunteer.Id.Value,
            pet.Id.Value,
            updatedSpeciesBreed.SpeciesId.Value,
            updatedSpeciesBreed.BreedId.Value,
            updatedName,
            updatedDescription,
            updatedColor,
            updatedHealthInformation,
            updatedCity,
            updatedStreet,
            updatedHouse,
            updatedWeight,
            updatedHeight,
            updatedPhoneNumber,
            updatedIsNeutered,
            updatedDateOnBirth,
            updatedIsVaccinated,
            updatedHelpStatus,
            updatedRequisites);

        //act
        var updatedResult = await ExecuteHandlers<UpdatePetHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(updatedResult.IsSuccess);
        
        Assert.Equal(pet.Id.Value, updatedResult.Value);

        var updatedPet = await ExecuteInDatabase(async context =>
        {
            return await context.PetsRead
                .FirstOrDefaultAsync(p => p.Id == pet.Id, cancellationToken);
        });
        
        Assert.NotNull(updatedPet);
        Assert.Equal(pet.Id, updatedPet.Id);
        
        Assert.NotEqual(pet.Name, updatedPet.Name);
        Assert.NotEqual(pet.Description, updatedPet.Description);
        Assert.NotEqual(pet.Color, updatedPet.Color);
        Assert.NotEqual(pet.SpeciesBreed, updatedPet.SpeciesBreed);
        Assert.NotEqual(pet.HealthInformation, updatedPet.HealthInformation);
        Assert.NotEqual(pet.Address, updatedPet.Address);
        Assert.NotEqual(pet.BodySize, updatedPet.BodySize);
        Assert.NotEqual(pet.PhoneNumber, updatedPet.PhoneNumber);
        Assert.NotEqual(pet.IsNeutered, updatedPet.IsNeutered);
        Assert.NotEqual(pet.IsVaccinated, updatedPet.IsVaccinated);
        Assert.NotEqual(pet.HelpStatus, updatedPet.HelpStatus);
        Assert.NotEqual(pet.DateOfBirth, updatedPet.DateOfBirth);
        Assert.NotEqual(pet.Requisites, updatedPet.Requisites);
    }
    
    [Fact]
    public async Task UpdateFullInfo_WithInvalidData_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var pet = await CreatePet("test-pet", cancellationToken);
        var volunteer = await CreateVolunteer([pet], cancellationToken);

        var updatedSpeciesBreed = await CreateSpeciesBreed(cancellationToken);
        var updatedName = string.Empty;
        var updatedDescription = string.Empty;
        var updatedColor = string.Empty;
        var updatedHealthInformation = string.Empty;
        var updatedCity = string.Empty;
        var updatedStreet = string.Empty;
        var updatedHouse = string.Empty;
        var updatedWeight = 0;
        var updatedHeight = 0;
        var updatedPhoneNumber = string.Empty;
        var updatedIsNeutered = false;
        DateOnly updatedDateOnBirth = DateOnly.FromDateTime(DateTime.Now);
        var updatedIsVaccinated = false;
        var updatedHelpStatus = string.Empty;
        var updatedRequisites = new List<RequisitesDto>
        {
            new()
            {
                AccountNumber = "1111111111111111111111111111111111111111",
                Description = "updated-description",
                Title = "updated-title"
            }
        };

        var command = new UpdatePetCommand(
            volunteer.Id.Value,
            pet.Id.Value,
            updatedSpeciesBreed.SpeciesId.Value,
            updatedSpeciesBreed.BreedId.Value,
            updatedName,
            updatedDescription,
            updatedColor,
            updatedHealthInformation,
            updatedCity,
            updatedStreet,
            updatedHouse,
            updatedWeight,
            updatedHeight,
            updatedPhoneNumber,
            updatedIsNeutered,
            updatedDateOnBirth,
            updatedIsVaccinated,
            updatedHelpStatus,
            updatedRequisites);

        //act
        var updatedResult = await ExecuteHandlers<UpdatePetHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(updatedResult.IsFailure);

        var nonUpdatedPet = await ExecuteInDatabase(async context =>
        {
            return await context.PetsRead
                .FirstOrDefaultAsync(p => p.Id == pet.Id, cancellationToken);
        });
        
        Assert.NotNull(nonUpdatedPet);
        Assert.Equal(pet.Id, nonUpdatedPet.Id);
        
        Assert.Equal(pet.Name, nonUpdatedPet.Name);
        Assert.Equal(pet.Description, nonUpdatedPet.Description);
        Assert.Equal(pet.Color, nonUpdatedPet.Color);
        Assert.Equal(pet.SpeciesBreed, nonUpdatedPet.SpeciesBreed);
        Assert.Equal(pet.HealthInformation, nonUpdatedPet.HealthInformation);
        Assert.Equal(pet.Address, nonUpdatedPet.Address);
        Assert.Equal(pet.BodySize, nonUpdatedPet.BodySize);
        Assert.Equal(pet.PhoneNumber, nonUpdatedPet.PhoneNumber);
        Assert.Equal(pet.IsNeutered, nonUpdatedPet.IsNeutered);
        Assert.Equal(pet.IsVaccinated, nonUpdatedPet.IsVaccinated);
        Assert.Equal(pet.HelpStatus, nonUpdatedPet.HelpStatus);
        Assert.Equal(pet.DateOfBirth, nonUpdatedPet.DateOfBirth);
        Assert.Equal(pet.Requisites, nonUpdatedPet.Requisites);
        
        Assert.Equal(9, updatedResult.Error.Count());
    }
}