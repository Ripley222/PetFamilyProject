/*using CSharpFunctionalExtensions;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures.DTOs;

namespace PetFamily.IntegrationTests.Volunteers;

public class GetByIdTests(WebTestsFactory factory) : VolunteersEntityFactory(factory)
{
    [Fact]
    public async Task GetById_WithValidId_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteer = await CreateVolunteer(cancellationToken);

        var query = new GetVolunteersByIdQuery(volunteer.Id.Value);
        
        //act
        var volunteerResult = await ExecuteHandlers<GetVolunteersByIdHandler, Result<VolunteerDto?, ErrorList>>(
            async sut => await sut.Handle(query, cancellationToken));

        //assert
        Assert.True(volunteerResult.IsSuccess);
        Assert.NotNull(volunteerResult.Value);
        
        Assert.Equal(volunteer.Id.Value, volunteerResult.Value.VolunteerId);
    }
    
    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnNull()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        var volunteer = await CreateVolunteer(cancellationToken);
        var invalidId = Guid.NewGuid();

        var query = new GetVolunteersByIdQuery(invalidId);
        
        //act
        var volunteerResult = await ExecuteHandlers<GetVolunteersByIdHandler, Result<VolunteerDto?, ErrorList>>(
            async sut => await sut.Handle(query, cancellationToken));

        //assert
        Assert.True(volunteerResult.IsSuccess);
        Assert.NotEqual(volunteer.Id.Value, invalidId);
        Assert.Null(volunteerResult.Value);
    }
}*/