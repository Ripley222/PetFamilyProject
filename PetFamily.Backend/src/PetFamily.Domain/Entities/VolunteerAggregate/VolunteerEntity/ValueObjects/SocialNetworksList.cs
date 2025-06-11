namespace PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;

public record SocialNetworksList
{
    public List<SocialNetwork>? SocialNetworks { get; private set; }
}