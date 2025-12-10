/*using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures.DTOs;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace PetFamily.IntegrationTests.Volunteers;

public class UpdateSocialNetworksTests(WebTestsFactory factory) : VolunteersEntityFactory(factory)
{
    [Fact]
    public async Task UpdateSocialNetworks_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteer = await CreateVolunteer(cancellationToken);

        var newSocialNetworks = new List<SocialNetworksDto>
        {
            new()
            {
                Link = "https://www.new-soical-test.com",
                Title = "New social network",
            },
            new()
            {
                Link = "https://www.new-socail-test.com",
                Title = "New social network",
            }
        };

        var command = new UpdateSocialNetworksCommand(volunteer.Id.Value, newSocialNetworks);

        //act
        var volunteerIdResult = await ExecuteHandlers<UpdateSocialNetworksHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(volunteerIdResult.IsSuccess);

        var updatedVolunteer = await ExecuteInDatabase(async context =>
        {
            return await context.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);
        });

        Assert.NotNull(updatedVolunteer);

        Assert.NotEqual(volunteer.Socials.Count, updatedVolunteer.Socials.Count);
        
        Assert.Equal(newSocialNetworks.Count, updatedVolunteer.Socials.Count);
    }
    
    [Fact]
    public async Task UpdateSocialNetworks_WithInvalidData_ShouldFailure()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteer = await CreateVolunteer(cancellationToken);

        var newSocialNetworks = new List<SocialNetworksDto>
        {
            new()
            {
                Link = new string('a', SocialNetwork.MAX_LENGTH_LINK + 1),
                Title = new string('a', Constants.MAX_LENGTH_TITLE + 1),
            }
        };

        var command = new UpdateSocialNetworksCommand(volunteer.Id.Value, newSocialNetworks);

        //act
        var volunteerIdResult = await ExecuteHandlers<UpdateSocialNetworksHandler, Result<Guid, ErrorList>>(
            async sut => await sut.Handle(command, cancellationToken));

        //assert
        Assert.True(volunteerIdResult.IsFailure);

        var updatedVolunteer = await ExecuteInDatabase(async context =>
        {
            return await context.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);
        });

        Assert.NotNull(updatedVolunteer);
        
        Assert.Equal(volunteer.Socials, updatedVolunteer.Socials);
    }
}*/