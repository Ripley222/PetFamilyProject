using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.VolunterAggregate.PetEntity;
using PetFamily.Domain.Entities.VolunterAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunterAggregate.VolunteerEntity.ValueObjects;

namespace PetFamily.Domain.Entities.VolunterAggregate.VolunteerEntity
{
    public class Volunteer : Entity<Guid>
    {
        private readonly List<Pet> _pets = [];

        private readonly List<SocialMedia> _socialMedias = [];

        //ef core ctor
        public Volunteer(){ }

        private Volunteer(
            Guid id,
            FullName fullName,
            EmailAddress emailAddress,
            string description,
            int yearsOfExperience,
            PhoneNumber phoneNumber,
            Requisities requisities)
        {
            Id = id;
            FullName = fullName;
            EmailAddress = emailAddress;
            Description = description;
            YearsOfExperience = yearsOfExperience;
            PhoneNumber = phoneNumber;
            Requisities = requisities;
        }

        public FullName FullName { get; private set; }
        public EmailAddress EmailAddress { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public int YearsOfExperience { get; private set; }
        public int NumberOfAnimalsFoundHome => FoundHomeAnimalsNumber();
        public int NumberOfAnimalsLookingHome => LookingHomeAnimalsNumber();
        public int NumberOfAnimalsNeedsHelp => NeedsHelpAnimalsNumber();
        public PhoneNumber PhoneNumber { get; private set; }
        public Requisities Requisities { get; private set; }
        public IReadOnlyList<SocialMedia> SocialMedias => _socialMedias;
        public IReadOnlyList<Pet> Pets => _pets;

        public static Result<Volunteer> Create(
            Guid id,
            FullName fullName,
            EmailAddress emailAddress,
            string description,
            int yearsOfExperience,
            PhoneNumber phoneNumber,
            Requisities requisities)
        {
            if (id == Guid.Empty)
                return Result.Failure<Volunteer>("Не задан индентификацонный номер волонтёра!");

            if (fullName is null)
                return Result.Failure<Volunteer>("Необходимо указать имя волонтёра!");

            if (emailAddress is null)
                return Result.Failure<Volunteer>("Необходимо указать электронный адрес волонтёра!");

            if (phoneNumber is null)
                return Result.Failure<Volunteer>("Необходимо указать номер телефона волонтёра!");

            if (requisities is null)
                return Result.Failure<Volunteer>("Необходимо указать реквизиты волонтёра!");

            var result = new Volunteer(id, fullName, emailAddress, description, yearsOfExperience, phoneNumber, requisities);

            return Result.Success(result);
        }

        public Result AddPet(Pet pet)
        {
            if (pet is null)
                return Result.Failure("Необходимо указать животное!");

            if (_pets.Contains(pet))
                return Result.Failure("Указанное животное уже числится за волонтёром!");

            _pets.Add(pet);

            return Result.Success();
        }

        public Result AddSocialMedia(SocialMedia socialMedia)
        {
            if (socialMedia is null)
                return Result.Failure("Необходимо указать социальную сеть!");

            if (_socialMedias.Contains(socialMedia))
                return Result.Failure("Указанное социальная сеть уже добавлена!");

            _socialMedias.Add(socialMedia);

            return Result.Success();
        }

        private int FoundHomeAnimalsNumber()
        {
            var result = _pets.Where(p => p.HelpStatus == HelpStatus.FoundHome).Count();
            return result;
        }

        private int LookingHomeAnimalsNumber()
        {
            var result = _pets.Where(p => p.HelpStatus == HelpStatus.LookingHome).Count();
            return result;
        }

        private int NeedsHelpAnimalsNumber()
        {
            var result = _pets.Where(p => p.HelpStatus == HelpStatus.NeedsHelp).Count();
            return result;
        }
    }
}
