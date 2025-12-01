using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Application.VolunteersFeatures.Update.Requisites;
using PetFamily.IntegrationTests.Handlers;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Volunteers;

public class UpdateRequisitesTests(WebTestsFactory factory) : ExecuteVolunteersHandlers(factory)
{
    [Fact]
    public async Task UpdateRequisitesInfo_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteer = await CreateVolunteer(cancellationToken);

        var newRequisites = new List<RequisitesDto>()
        {
            new()
            {
                AccountNumber = "00000000000000000001",
                Description = "New requisite",
                Title = "New requisite"
            },
            new()
            {
                AccountNumber = "00000000000000000002",
                Description = "New requisite",
                Title = "New requisite"
            }
        };

        var command = new UpdateRequisitesCommand(volunteer.Id.Value, newRequisites);

        //act
        var volunteerIdResult = await ExecuteUpdateRequisitesHandler(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(volunteerIdResult.IsSuccess);
        
        var updatedVolunteer = await ExecuteInDatabase(async context =>
        {
            return await context.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);
        });
        
        Assert.NotNull(updatedVolunteer);
        
        Assert.NotEqual(volunteer.Requisites, updatedVolunteer.Requisites);
        
        Assert.Equal(newRequisites.Count, updatedVolunteer.Requisites.Count);
    }
    
    [Fact]
    public async Task UpdateRequisitesInfo_WithInvalidData_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteer = await CreateVolunteer(cancellationToken);

        var newRequisites = new List<RequisitesDto>()
        {
            new()
            {
                AccountNumber = "01",
                Description = "New requisite",
                Title = "New requisite"
            },
            new()
            {
                AccountNumber = "0000000000002",
                Description = string.Empty,
                Title = string.Empty
            }
        };

        var command = new UpdateRequisitesCommand(volunteer.Id.Value, newRequisites);

        //act
        var volunteerIdResult = await ExecuteUpdateRequisitesHandler(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(volunteerIdResult.IsFailure);
        
        var updatedVolunteer = await ExecuteInDatabase(async context =>
        {
            return await context.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);
        });
        
        Assert.NotNull(updatedVolunteer);
        
        Assert.Equal(volunteer.Requisites, updatedVolunteer.Requisites);
    }
}