using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteersFeatures.Create;
using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Volunteers;

public class CreateTests(WebTestsFactory webTestsFactory) : VolunteersEntityFactory(webTestsFactory)
{
    [Fact]
    public async Task CreateVolunteer_WithValidData_ShouldSuccess()
    {
        //arrange
        var yearsOfExperience = 1;

        var requisites = new List<RequisitesDto>()
        {
            new()
            {
                AccountNumber = "00000000000000000000",
                Title = "test",
                Description = "test",
            }
        };

        var socials = new List<SocialNetworksDto>()
        {
            new()
            {
                Link = "test-link",
                Title = "test"
            }
        };

        var cancellationToken = CancellationToken.None;

        //act
        var volunteerIdResult = await ExecuteHandlers<CreateVolunteerHandler, Result<Guid, ErrorList>>(async sut =>
        {
            var command = new CreateVolunteerCommand(
                "test",
                "test",
                "test",
                "test@gmail.com",
                "test",
                yearsOfExperience,
                "+79788339045",
                requisites,
                socials);

            return await sut.Handle(command, cancellationToken);
        });

        //assert
        var volunteer = await ExecuteInDatabase(async dbContext =>
        {
            return await dbContext.Volunteers
                .FirstOrDefaultAsync(
                    v => v.Id == VolunteerId.Create(volunteerIdResult.Value),
                    cancellationToken);
        });

        Assert.NotNull(volunteer);
        Assert.Equal(volunteerIdResult.Value, volunteer.Id.Value);

        Assert.True(volunteerIdResult.IsSuccess);
        Assert.NotEqual(Guid.Empty, volunteerIdResult);
    }

    [Fact]
    public async Task CreateVolunteers_WithDuplicate_ShouldFailure()
    {
        //arrange
        var yearsOfExperience = 1;

        var requisites = new List<RequisitesDto>()
        {
            new()
            {
                AccountNumber = "00000000000000000000",
                Title = "test",
                Description = "test",
            }
        };

        var socials = new List<SocialNetworksDto>()
        {
            new()
            {
                Link = "test-link",
                Title = "test"
            }
        };

        var cancellationToken = CancellationToken.None;

        //act
        await ExecuteHandlers<CreateVolunteerHandler, Result<Guid, ErrorList>>(sut =>
        {
            var command = new CreateVolunteerCommand(
                "test",
                "test",
                "test",
                "test@gmail.com",
                "test",
                yearsOfExperience,
                "+79788339045",
                requisites,
                socials);

            return sut.Handle(command, cancellationToken);
        });
        
        var volunteerResultWithError = await ExecuteHandlers<CreateVolunteerHandler, Result<Guid, ErrorList>>(sut =>
        {
            var command = new CreateVolunteerCommand(
                "test",
                "test",
                "test",
                "test@gmail.com",
                "test",
                yearsOfExperience,
                "+79788339045",
                requisites,
                socials);

            return sut.Handle(command, cancellationToken);
        });

        //assert
        Assert.True(volunteerResultWithError.IsFailure);
        Assert.NotEmpty(volunteerResultWithError.Error);
    }
}