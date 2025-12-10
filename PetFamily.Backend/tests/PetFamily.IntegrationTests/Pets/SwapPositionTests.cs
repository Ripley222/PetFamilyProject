/*using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;
using SharedKernel;

namespace PetFamily.IntegrationTests.Pets;

public class SwapPositionTests(WebTestsFactory webTestsFactory) : PetsEntityFactory(webTestsFactory)
{
    [Fact]
    public async Task MovePet_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;
        
        var firstPet = await CreatePet("first-pet", cancellationToken);
        var secondPet = await CreatePet("second-pet", cancellationToken);
        var thirdPet = await CreatePet("third-pet", cancellationToken);
        
        var volunteerForPet = await CreateVolunteer([firstPet, secondPet, thirdPet], cancellationToken);

        const int firstPosition = 1;
        const int secondPosition = 2;
        const int thirdPosition = 3;
        
        var command = new MovePetCommand(
            volunteerForPet.Id.Value,
            thirdPet.Id.Value,
            firstPosition);

        //act
        var petIdResult = await ExecuteHandlers<MovePetHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        var pets = await ExecuteInDatabase(async dbContext =>
        {
            var volunteer = await dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteerForPet.Id, cancellationToken);

            return volunteer?.Pets.OrderBy(p => p.Position.Value).ToList();
        });
        
        Assert.True(petIdResult.IsSuccess);
        Assert.NotNull(pets);
        
        var firstPetAssert = pets.FirstOrDefault(p => p.Name == firstPet.Name);
        var secondPetAssert = pets.FirstOrDefault(p => p.Name == secondPet.Name);
        var thirdPetAssert = pets.FirstOrDefault(p => p.Name == thirdPet.Name);
        
        Assert.NotNull(firstPetAssert);
        Assert.NotNull(secondPetAssert);
        Assert.NotNull(thirdPetAssert);
        
        Assert.Equal(firstPosition, thirdPetAssert.Position.Value);
        Assert.Equal(secondPosition, firstPetAssert.Position.Value);
        Assert.Equal(thirdPosition, secondPetAssert.Position.Value);
    }
    
    [Fact]
    public async Task MovePet_WithInvalidPosition_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None; ;
        
        var firstPet = await CreatePet("first-pet", cancellationToken);
        var secondPet = await CreatePet("second-pet", cancellationToken);
        var thirdPet = await CreatePet("third-pet", cancellationToken);
        
        var volunteerForPet = await CreateVolunteer([firstPet, secondPet, thirdPet], cancellationToken);

        const int firstPosition = 1;
        const int secondPosition = 2;
        const int thirdPosition = 3;
        const int fourthPosition = 4;
        
        var command = new MovePetCommand(
            volunteerForPet.Id.Value,
            firstPet.Id.Value,
            fourthPosition);

        //act
        var petIdResult = await ExecuteHandlers<MovePetHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        var pets = await ExecuteInDatabase(async dbContext =>
        {
            var volunteer = await dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteerForPet.Id, cancellationToken);

            return volunteer?.Pets.OrderBy(p => p.Position.Value).ToList();
        });
        
        Assert.True(petIdResult.IsFailure);
        Assert.NotNull(pets);
        
        var firstPetAssert = pets.FirstOrDefault(p => p.Name == firstPet.Name);
        var secondPetAssert = pets.FirstOrDefault(p => p.Name == secondPet.Name);
        var thirdPetAssert = pets.FirstOrDefault(p => p.Name == thirdPet.Name);
        
        Assert.NotNull(firstPetAssert);
        Assert.NotNull(secondPetAssert);
        Assert.NotNull(thirdPetAssert);
        
        Assert.Equal(firstPosition, firstPetAssert.Position.Value);
        Assert.Equal(secondPosition, secondPetAssert.Position.Value);
        Assert.Equal(thirdPosition, thirdPetAssert.Position.Value);
    }
}*/