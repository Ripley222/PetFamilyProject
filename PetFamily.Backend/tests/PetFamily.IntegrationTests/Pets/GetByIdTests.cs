/*using CSharpFunctionalExtensions;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures.DTOs;

namespace PetFamily.IntegrationTests.Pets;

public class GetByIdTests(WebTestsFactory testsFactory) : PetsEntityFactory(testsFactory)
{
    [Fact]
    public async Task GetPetById_WithValidId_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var pet = await CreatePet("test-pet", cancellationToken);
        var volunteerForPet = await CreateVolunteer([pet], cancellationToken);

        var query = new GetPetsByIdQuery(pet.Id.Value);

        //act
        var petResult = await ExecuteHandlers<GetPetsByIdHandler, Result<PetDto, ErrorList>>(async sut =>
            await sut.Handle(query, cancellationToken));

        //assert
        Assert.True(petResult.IsSuccess);
        Assert.Equal(petResult.Value.PetId, pet.Id.Value);
        Assert.Equal(petResult.Value.VolunteerId, volunteerForPet.Id.Value);
    }
    
    [Fact]
    public async Task GetPetById_WithInvalidId_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var pet = await CreatePet("test-pet", cancellationToken);
        await CreateVolunteer([pet], cancellationToken);

        var invalidId = Guid.NewGuid();
        
        var query = new GetPetsByIdQuery(invalidId);

        //act
        var petResult = await ExecuteHandlers<GetPetsByIdHandler, Result<PetDto, ErrorList>>(async sut =>
            await sut.Handle(query, cancellationToken));

        //assert
        Assert.True(petResult.IsFailure);
    }
}*/