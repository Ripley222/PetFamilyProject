/*using PetFamily.IntegrationTests.Infrastructure;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace PetFamily.IntegrationTests.Entities;

public class VolunteersEntityFactory(WebTestsFactory webTestsFactory) : BaseForTests(webTestsFactory)
{
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
}*/