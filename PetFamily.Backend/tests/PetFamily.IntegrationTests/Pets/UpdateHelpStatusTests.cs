using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Update.Status;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Pets;

public class UpdateHelpStatusTests(WebTestsFactory testsFactory) : PetsEntityFactory(testsFactory)
{
    [Fact]
    public async Task UpdateHelpStatus_WithValidStatus_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;
        
        var pet = await CreatePet("test-pet", cancellationToken);
        var volunteer = await CreateVolunteer([pet], cancellationToken);

        var updatedHelpStatus = HelpStatus.NeedsHelp.Value;
        
        var command = new UpdatePetStatusCommand(volunteer.Id.Value, pet.Id.Value, updatedHelpStatus);
        
        //act
        var updatedResult = await ExecuteHandlers<UpdatePetStatusHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(updatedResult.IsSuccess);

        var updatedPet = await ExecuteInDatabase(async context =>
        {
            return await context.PetsRead
                .FirstOrDefaultAsync(p => p.Id == pet.Id, cancellationToken);
        });
        
        Assert.NotNull(updatedPet);
        Assert.NotEqual(pet.HelpStatus, updatedPet.HelpStatus);
        Assert.Equal(updatedPet.HelpStatus, HelpStatus.Create(updatedHelpStatus).Value);
    }
    
    [Fact]
    public async Task UpdateHelpStatus_WithInvalidStatus_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;
        
        var pet = await CreatePet("test-pet", cancellationToken);
        var volunteer = await CreateVolunteer([pet], cancellationToken);

        var updatedHelpStatus = string.Empty;
        
        var command = new UpdatePetStatusCommand(volunteer.Id.Value, pet.Id.Value, updatedHelpStatus);
        
        //act
        var updatedResult = await ExecuteHandlers<UpdatePetStatusHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(updatedResult.IsFailure);

        var updatedPet = await ExecuteInDatabase(async context =>
        {
            return await context.PetsRead
                .FirstOrDefaultAsync(p => p.Id == pet.Id, cancellationToken);
        });
        
        Assert.NotNull(updatedPet);
        Assert.Equal(pet.HelpStatus, updatedPet.HelpStatus);
    }
}