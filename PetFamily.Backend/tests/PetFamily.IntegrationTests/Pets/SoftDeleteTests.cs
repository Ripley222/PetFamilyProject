using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Delete;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.IntegrationTests.Handlers;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Pets;

public class SoftDeleteTests(WebTestsFactory testsFactory) : ExecutePetsHandlers(testsFactory)
{
    [Fact]
    public async Task SoftDeletePet_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var pet = await CreatePet("test-pet", cancellationToken);

        var volunteerForPet = await CreateVolunteer([pet], cancellationToken);
        
        var command = new DeletePetCommand(volunteerForPet.Id.Value, pet.Id.Value);

        //act
        var softDeletedResult = await ExecuteSofDeleteHandlers(async sut =>
            await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(softDeletedResult.IsSuccess);

        var softDeletedPet = await ExecuteInDatabase(async context =>
        {
            var pet = await context.PetsRead
                .Where(p => EF.Property<bool>(p, "_isDeleted") == true)
                .FirstOrDefaultAsync(cancellationToken);

            return pet;
        });
        
        Assert.NotNull(softDeletedPet);
        Assert.Equal(pet.Id, softDeletedPet.Id);
    }
    
    [Fact]
    public async Task SoftDeletePet_WithInvalidPetId_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var pet = await CreatePet("test-pet", cancellationToken);

        var volunteerForPet = await CreateVolunteer([pet], cancellationToken);
        
        var command = new DeletePetCommand(volunteerForPet.Id.Value, PetId.New().Value);

        //act
        var softDeletedResult = await ExecuteSofDeleteHandlers(async sut =>
            await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(softDeletedResult.IsFailure);

        var softDeletedPet = await ExecuteInDatabase(async context =>
        {
            var pet = await context.PetsRead
                .Where(p => EF.Property<bool>(p, "_isDeleted") == true)
                .FirstOrDefaultAsync(cancellationToken);

            return pet;
        });
        
        Assert.Null(softDeletedPet);
    }
}