using CSharpFunctionalExtensions;
using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Application.VolunteersFeatures.PetFeatures.GetAll;
using PetFamily.Domain.Shared;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Pets;

public class GetAllTests(WebTestsFactory testsFactory) : PetsEntityFactory(testsFactory)
{
    [Fact]
    public async Task GetAllPetsByVolunteerId_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;
        
        var firstPet = await CreatePet("test-pet", cancellationToken);
        var secondPet = await CreatePet("test-pet", cancellationToken);
        var thirdPet = await CreatePet("test-pet", cancellationToken);
        
        var volunteerForPets = await CreateVolunteer([firstPet, secondPet, thirdPet], cancellationToken);

        var query = new GetPetsQuery(
            volunteerForPets.Id.Value,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        //act
        var petsResult = await ExecuteHandlers<GetPetsHandler, Result<IReadOnlyList<PetDto>, ErrorList>>(
            async sut => await sut.Handle(query, cancellationToken));

        //assert
        Assert.True(petsResult.IsSuccess);
        Assert.Equal(volunteerForPets.Pets.Count, petsResult.Value.Count);
    }
    
    [Fact]
    public async Task GetAllPetsByPetName_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var petsName = "test-pet";
        
        var firstPet = await CreatePet(petsName, cancellationToken);
        var secondPet = await CreatePet(petsName, cancellationToken);
        var thirdPet = await CreatePet(petsName, cancellationToken);
        
        var volunteerForPets = await CreateVolunteer([firstPet, secondPet, thirdPet], cancellationToken);

        var query = new GetPetsQuery(
            null,
            petsName,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        //act
        var petsResult = await ExecuteHandlers<GetPetsHandler, Result<IReadOnlyList<PetDto>, ErrorList>>(
            async sut => await sut.Handle(query, cancellationToken));

        //assert
        Assert.True(petsResult.IsSuccess);
        Assert.Equal(volunteerForPets.Pets.Count, petsResult.Value.Count);
    }
    
    [Fact]
    public async Task GetAllPetsByVolunteerId_WithInvalidData_ShouldEmptyResultValue()
    {
        //arrange
        var cancellationToken = CancellationToken.None;
        
        var firstPet = await CreatePet("test-pet", cancellationToken);
        var secondPet = await CreatePet("test-pet", cancellationToken);
        var thirdPet = await CreatePet("test-pet", cancellationToken);
        
        var volunteerForPets = await CreateVolunteer([firstPet, secondPet, thirdPet], cancellationToken);

        var invalidVolunteerId = Guid.NewGuid();

        var query = new GetPetsQuery(
            invalidVolunteerId,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        //act
        var petsResult = await ExecuteHandlers<GetPetsHandler, Result<IReadOnlyList<PetDto>, ErrorList>>(
            async sut => await sut.Handle(query, cancellationToken));

        //assert
        Assert.True(petsResult.IsSuccess);
        Assert.Empty(petsResult.Value);
        Assert.NotEqual(volunteerForPets.Pets.Count, petsResult.Value.Count);
    }
    
    [Fact]
    public async Task GetAllPetsByVolunteerId_WithInvalidData_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;
        
        var firstPet = await CreatePet("test-pet", cancellationToken);
        var secondPet = await CreatePet("test-pet", cancellationToken);
        var thirdPet = await CreatePet("test-pet", cancellationToken);
        
        await CreateVolunteer([firstPet, secondPet, thirdPet], cancellationToken);

        var emptyVolunteerId = Guid.Empty;

        var query = new GetPetsQuery(
            emptyVolunteerId, 
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        //act
        var petsResult = await ExecuteHandlers<GetPetsHandler, Result<IReadOnlyList<PetDto>, ErrorList>>(
            async sut => await sut.Handle(query, cancellationToken));

        //assert
        Assert.True(petsResult.IsFailure);
    }
}