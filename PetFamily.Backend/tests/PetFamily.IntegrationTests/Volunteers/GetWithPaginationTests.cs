/*using CSharpFunctionalExtensions;
using PetFamily.IntegrationTests.Entities;
using PetFamily.IntegrationTests.Infrastructure;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures.DTOs;

namespace PetFamily.IntegrationTests.Volunteers;

public class GetWithPaginationTests(WebTestsFactory factory) : VolunteersEntityFactory(factory)
{
    [Fact]
    public async Task GetWithPagination_WithValidData_ShouldSuccess()
    {
        //arrange
        var cancellationToken = CancellationToken.None;

        int page = 1;
        int pageSize = 3;
        
        var query = new GetVolunteersQuery(page, pageSize);

        await CreateVolunteer(cancellationToken);
        await CreateVolunteer(cancellationToken);
        await CreateVolunteer(cancellationToken);
        await CreateVolunteer(cancellationToken);

        //act
        var volunteersResult = await ExecuteHandlers<GetVolunteersHandler, Result<GetVolunteersDto?, ErrorList>>(
            async sut => await sut.Handle(query, cancellationToken));

        //assert
        Assert.True(volunteersResult.IsSuccess);
        
        Assert.NotNull(volunteersResult.Value);
        
        Assert.Equal(pageSize, volunteersResult.Value.Volunteers.Count());
    }
}*/