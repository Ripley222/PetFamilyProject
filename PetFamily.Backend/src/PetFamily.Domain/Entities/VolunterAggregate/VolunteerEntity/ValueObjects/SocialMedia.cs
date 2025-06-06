using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.VolunterAggregate.VolunteerEntity.ValueObjects
{
    public class SocialMedia : ValueObject
    {
        public string Name { get; }
        public string Link { get; }

        private SocialMedia(string name, string link)
        {
            Name = name;
            Link = link;
        }

        public static Result<SocialMedia> Create(string name, string link)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<SocialMedia>("Необходимо указать название социальной сети!");

            if (string.IsNullOrWhiteSpace(link))
                return Result.Failure<SocialMedia>("Необходимо указать ссылку на социальную сеть!");

            var result = new SocialMedia(name, link);

            return Result.Success(result);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Link;
        }
    }
}
