using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects
{
    public record SocialNetwork
    {
        public const int MAX_LENGTH_LINK = 200;
        
        public string Title { get; }
        public string Link { get; }

        private SocialNetwork(string title, string link)
        {
            Title = title;
            Link = link;
        }

        public static Result<SocialNetwork> Create(string name, string link)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<SocialNetwork>("Необходимо указать название социальной сети!");

            if (string.IsNullOrWhiteSpace(link))
                return Result.Failure<SocialNetwork>("Необходимо указать ссылку на социальную сеть!");

            var socialMedia = new SocialNetwork(name, link);

            return Result.Success(socialMedia);
        }
    }
}
