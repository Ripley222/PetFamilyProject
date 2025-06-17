using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects
{
    public record SocialNetwork
    {
        public const int MAX_LENGTH_LINK = 200;
        
        public string Title { get; } = string.Empty;
        public string Link { get; } = string.Empty;
        
        private SocialNetwork(string title, string link)
        {
            Title = title;
            Link = link;
        }

        public static Result<SocialNetwork, Error> Create(string title, string link)
        {
            if (title.Length > Constants.MAX_LENGTH_TITLE)
               return Errors.General.ValueIsInvalid("Title");

            if (link.Length > MAX_LENGTH_LINK)
                return Errors.General.ValueIsInvalid("Link");

            return new SocialNetwork(title, link);
        }
    }
}
