namespace PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;

public record SocialNetworksList
{
    public IEnumerable<SocialNetwork> SocialNetworks { get; }

    //ef core ctor
    public SocialNetworksList()
    {
    }
    
    private SocialNetworksList(IEnumerable<SocialNetwork> socialNetworks)
    {
        SocialNetworks =  socialNetworks;
    }

    public static SocialNetworksList Create(IEnumerable<SocialNetwork> socialNetworks)
    {
        return new SocialNetworksList(socialNetworks);
    }
}