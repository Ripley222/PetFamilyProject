using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteersFeatures.Update.MainInfo;
using PetFamily.IntegrationTests.Handlers;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Volunteers;

public class UpdateMainInfoTests(WebTestsFactory factory) : ExecuteVolunteersHandlers(factory)
{
    [Fact]
    public async Task UpdateMainInfo_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteer = await CreateVolunteer(cancellationToken);

        string newFirstName = "NewTestFirstName";
        string newMiddleName = "NewTestMiddleName";
        string newLastName = "NewTestFirstName";
        string newEmailAddress = "NewEmailAddress@gmail.com";
        string newDescription = "NewDescription";
        int newYearsOfExperience = 5;
        string newPhoneNumber = "+79788339090";

        var command = new UpdateMainInfoCommand(
            volunteer.Id.Value,
            newFirstName,
            newMiddleName,
            newLastName,
            newEmailAddress,
            newDescription,
            newYearsOfExperience,
            newPhoneNumber);

        //act
        var volunteerIdResult =
            await ExecuteUpdateMainInfoHandler(async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(volunteerIdResult.IsSuccess);

        var updatedVolunteer = await ExecuteInDatabase(async context =>
        {
            return await context.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);
        });

        Assert.NotNull(updatedVolunteer);

        Assert.Equal(volunteerIdResult.Value, updatedVolunteer.Id.Value);

        Assert.NotEqual(volunteer.FullName, updatedVolunteer.FullName);
        Assert.NotEqual(volunteer.EmailAddress, updatedVolunteer.EmailAddress);
        Assert.NotEqual(volunteer.Description, updatedVolunteer.Description);
        Assert.NotEqual(volunteer.YearsOfExperience, updatedVolunteer.YearsOfExperience);
        Assert.NotEqual(volunteer.PhoneNumber, updatedVolunteer.PhoneNumber);
    }

    [Fact]
    public async Task UpdateMainInfo_WithInvalidData_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteer = await CreateVolunteer(cancellationToken);

        string newFirstName = string.Empty;
        string newMiddleName = string.Empty;
        string newLastName = string.Empty;
        string newEmailAddress = string.Empty;
        string newDescription = string.Empty;
        int newYearsOfExperience = -5;
        string newPhoneNumber = string.Empty;

        var command = new UpdateMainInfoCommand(
            volunteer.Id.Value,
            newFirstName,
            newMiddleName,
            newLastName,
            newEmailAddress,
            newDescription,
            newYearsOfExperience,
            newPhoneNumber);

        //act
        var volunteerIdResult =
            await ExecuteUpdateMainInfoHandler(async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(volunteerIdResult.IsFailure);

        var updatedVolunteer = await ExecuteInDatabase(async context =>
        {
            return await context.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);
        });

        Assert.NotNull(updatedVolunteer);
        
        Assert.Equal(volunteer.FullName, updatedVolunteer.FullName);
        Assert.Equal(volunteer.EmailAddress, updatedVolunteer.EmailAddress);
        Assert.Equal(volunteer.Description, updatedVolunteer.Description);
        Assert.Equal(volunteer.YearsOfExperience, updatedVolunteer.YearsOfExperience);
        Assert.Equal(volunteer.PhoneNumber, updatedVolunteer.PhoneNumber);
    }
}