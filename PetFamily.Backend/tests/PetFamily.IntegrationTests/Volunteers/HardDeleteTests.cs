using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteersFeatures.Delete;
using PetFamily.Application.VolunteersFeatures.Delete.HardDelete;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Volunteers;

public class HardDeleteTests(WebTestsFactory webTestsFactory) : VolunteersEntityFactory(webTestsFactory)
{
    [Fact]
    public async Task HardDelete_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var firstVolunteer = await CreateVolunteer(cancellationToken);

        var command = new DeleteVolunteerCommand(firstVolunteer.Id.Value);

        //act
        var deletionResult = await ExecuteHandlers<HardDeleteVolunteerHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(deletionResult.IsSuccess);
        Assert.Equal(firstVolunteer.Id.Value, deletionResult.Value);

        var firstExistsVolunteer = await ExecuteInDatabase(async context =>
        {
            return await context.Volunteers
                .FirstOrDefaultAsync(v => v.Id == firstVolunteer.Id, cancellationToken);
        });
        
        Assert.Null(firstExistsVolunteer);
    }
    
    [Fact]
    public async Task HardDelete_WithInvalidData_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteer = await CreateVolunteer(cancellationToken);

        var invalidId = VolunteerId.New();
        var command = new DeleteVolunteerCommand(invalidId.Value);

        //act
        var deletionResult = await ExecuteHandlers<HardDeleteVolunteerHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(deletionResult.IsFailure);

        var existingVolunteer = await ExecuteInDatabase(async context =>
        {
            return await context.Volunteers
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);
        });
        
        Assert.NotNull(existingVolunteer);
    }
}