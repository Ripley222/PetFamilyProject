using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.VolunteersFeatures.Create;
using PetFamily.Application.VolunteersFeatures.Delete.HardDelete;
using PetFamily.Application.VolunteersFeatures.Delete.SoftDelete;
using PetFamily.Application.VolunteersFeatures.GetById;
using PetFamily.Application.VolunteersFeatures.GetWithPagination;
using PetFamily.Application.VolunteersFeatures.Update.MainInfo;
using PetFamily.Application.VolunteersFeatures.Update.Requisites;
using PetFamily.Application.VolunteersFeatures.Update.SocialNetworks;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.IntegrationTests.Infrastructure;

namespace PetFamily.IntegrationTests.Handlers;

public class ExecuteVolunteersHandlers(WebTestsFactory webTestsFactory) : BaseForTests(webTestsFactory)
{
    protected async Task<T> ExecuteCreateHandler<T>(Func<CreateVolunteerHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<CreateVolunteerHandler>();

        return await action(sut);
    }

    protected async Task<T> ExecuteGetWithPaginationHandler<T>(Func<GetVolunteersHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<GetVolunteersHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteGetByIdHandler<T>(Func<GetVolunteersByIdHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<GetVolunteersByIdHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteUpdateMainInfoHandler<T>(Func<UpdateMainInfoHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<UpdateMainInfoHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteUpdateRequisitesHandler<T>(Func<UpdateRequisitesHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<UpdateRequisitesHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteUpdateSocialNetworksHandler<T>(Func<UpdateSocialNetworksHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<UpdateSocialNetworksHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteHardDeleteHandler<T>(Func<HardDeleteVolunteerHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<HardDeleteVolunteerHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteSoftDeleteHandler<T>(Func<SoftDeleteVolunteerHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<SoftDeleteVolunteerHandler>();
        
        return await action(sut);
    }
    
    protected async Task<Volunteer> CreateVolunteer(CancellationToken cancellationToken = default)
    {
        return await ExecuteInDatabase(async dbContext =>
        {
            var volunteerForPet = Volunteer.Create(
                VolunteerId.New(),
                FullName.Create("test", "test", "test").Value,
                EmailAddress.Create("test@gmail.com").Value,
                Description.Create("test").Value,
                1,
                PhoneNumber.Create("+79788339045").Value,
                new List<Requisite>()
                {
                    Requisite.Create("00000000000000000000", "test", "test").Value
                },
                new List<SocialNetwork>()
                {
                    SocialNetwork.Create("test", "http://www.test.com").Value
                });

            await dbContext.Volunteers.AddAsync(volunteerForPet.Value, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return volunteerForPet.Value;
        });
    }
}