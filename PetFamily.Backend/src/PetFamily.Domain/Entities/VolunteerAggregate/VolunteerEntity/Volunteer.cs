using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity
{
    public class Volunteer : Entity<VolunteerId>
    {
        private readonly List<Pet> _pets = [];

        //ef core ctor
        private Volunteer(VolunteerId id) : base(id)
        {
        }

        private Volunteer(
            VolunteerId id,
            FullName fullName,
            EmailAddress emailAddress,
            Description description,
            int yearsOfExperience,
            PhoneNumber phoneNumber)  : base(id)
        {
            FullName = fullName;
            EmailAddress = emailAddress;
            Description = description;
            YearsOfExperience = yearsOfExperience;
            PhoneNumber = phoneNumber;
        }

        public FullName FullName { get; private set; }
        public EmailAddress EmailAddress { get; private set; }
        public Description Description { get; private set; }
        public int YearsOfExperience { get; private set; }        
        public PhoneNumber PhoneNumber { get; private set; }
        public RequisitesList? RequisitesDetails { get; private set; }
        public SocialNetworksList? SocialNetworksDetails { get; private set; }
        
        public IReadOnlyList<Pet> Pets => _pets;
        public int GetNumberOfAnimalsFoundHome() => _pets.Count(p => p.HelpStatus == HelpStatus.FoundHome);
        public int GetNumberOfAnimalsLookingHome() => _pets.Count(p => p.HelpStatus == HelpStatus.LookingHome);
        public int GetNumberOfAnimalsNeedsHelp() => _pets.Count(p => p.HelpStatus == HelpStatus.NeedsHelp);

        public static Result<Volunteer> Create(
            VolunteerId volunteerId,
            FullName fullName,
            EmailAddress emailAddress,
            Description description,
            int yearsOfExperience,
            PhoneNumber phoneNumber)
        {
            var result = new Volunteer(volunteerId, fullName, emailAddress, description, yearsOfExperience, phoneNumber);

            return Result.Success(result);
        }

        public Result AddPet(Pet pet)
        {
            if (_pets.Contains(pet))
                return Result.Failure("Указанное животное уже числится за волонтёром!");

            _pets.Add(pet);

            return Result.Success();
        }
    }
}
