using System.Text.Json.Serialization;

namespace PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;

public record SocialNetworksList
{
    public List<SocialNetwork> SocialNetworks { get; } = [];

    [JsonConstructor]
    public SocialNetworksList()
    {
    }
    
    public SocialNetworksList(List<SocialNetwork> socialNetworks)
    {
        SocialNetworks =  socialNetworks;
    }
}