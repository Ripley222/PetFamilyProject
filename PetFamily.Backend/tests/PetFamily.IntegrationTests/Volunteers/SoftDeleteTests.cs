using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteersFeatures.Delete;
using PetFamily.Application.VolunteersFeatures.Delete.SoftDelete;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Volunteers;

public class SoftDeleteTests(WebTestsFactory webTestsFactory) : VolunteersEntityFactory(webTestsFactory)
{
    [Fact]
    public async Task SoftDelete_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteer = await CreateVolunteer(cancellationToken);

        var command = new DeleteVolunteerCommand(volunteer.Id.Value);

        //act
        var deletionResult = await ExecuteHandlers<SoftDeleteVolunteerHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(deletionResult.IsSuccess);
        Assert.Equal(volunteer.Id.Value, deletionResult.Value);

        var deletingVolunteer = await ExecuteInDatabase(async context =>
        {
            return await context.Volunteers
                .Where(v => EF.Property<bool>(v, "_isDeleted") == true)
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);
        });

        Assert.NotNull(deletingVolunteer);
    }
    
    [Fact]
    public async Task SoftDelete_WithInvalidData_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteer = await CreateVolunteer(cancellationToken);

        var invalidId = VolunteerId.New();
        var command = new DeleteVolunteerCommand(invalidId.Value);

        //act
        var deletionResult = await ExecuteHandlers<SoftDeleteVolunteerHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(deletionResult.IsFailure);

        var deletingVolunteer = await ExecuteInDatabase(async context =>
        {
            return await context.Volunteers
                .Where(v => EF.Property<bool>(v, "_isDeleted") == true)
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);
        });

        Assert.Null(deletingVolunteer);
    }
}