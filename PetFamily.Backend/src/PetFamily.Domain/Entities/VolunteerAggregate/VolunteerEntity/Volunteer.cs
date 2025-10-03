using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity
{
    public class Volunteer : Entity<VolunteerId>
    {
        public const int MIN_EXPERIENCE = 0;
        public const int MAX_EXPERIENCE = 50;

        private bool _isDeleted = false;

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
            PhoneNumber phoneNumber,
            List<Requisite> requisites,
            List<SocialNetwork> socials) : base(id)
        {
            FullName = fullName;
            EmailAddress = emailAddress;
            Description = description;
            YearsOfExperience = yearsOfExperience;
            PhoneNumber = phoneNumber;
            Requisites = requisites;
            Socials = socials;
        }

        public FullName FullName { get; private set; } = null!;
        public EmailAddress EmailAddress { get; private set; } = null!;
        public Description Description { get; private set; } = null!;
        public int YearsOfExperience { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; } = null!;
        public List<Requisite> Requisites { get; private set; } = null!;
        public List<SocialNetwork> Socials { get; private set; } = null!;
        public IReadOnlyList<Pet> Pets => _pets;
        public int GetNumberOfAnimalsFoundHome() => _pets.Count(p => p.HelpStatus == HelpStatus.FoundHome);
        public int GetNumberOfAnimalsLookingHome() => _pets.Count(p => p.HelpStatus == HelpStatus.LookingHome);
        public int GetNumberOfAnimalsNeedsHelp() => _pets.Count(p => p.HelpStatus == HelpStatus.NeedsHelp);

        public static Result<Volunteer, Error> Create(
            VolunteerId volunteerId,
            FullName fullName,
            EmailAddress emailAddress,
            Description description,
            int yearsOfExperience,
            PhoneNumber phoneNumber,
            List<Requisite> requisites,
            List<SocialNetwork> socials)
        {
            return new Volunteer(
                volunteerId,
                fullName,
                emailAddress,
                description,
                yearsOfExperience,
                phoneNumber,
                requisites,
                socials);
        }

        public void UpdateMainInfo(
            FullName fullName,
            EmailAddress emailAddress,
            Description description,
            int yearsOfExperience,
            PhoneNumber phoneNumber)
        {
            FullName = fullName;
            EmailAddress = emailAddress;
            Description = description;
            YearsOfExperience = yearsOfExperience;
            PhoneNumber = phoneNumber;
        }

        public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socials)
        {
            Socials = socials.ToList();
        }

        public void UpdateRequisites(IEnumerable<Requisite> requisites)
        {
            Requisites = requisites.ToList();
        }

        public void Delete()
        {
            _isDeleted = true;

            foreach (var pet in _pets)
            {
                pet.Delete();
            }
        }

        public void Restore()
        {
            _isDeleted = false;

            foreach (var pet in _pets)
            {
                pet.Restore();
            }
        }

        public Result<Pet, Error> GetPetById(PetId petId)
        {
            var pet = _pets.FirstOrDefault(p => p.Id == petId);

            if (pet is null)
                return Error.Failure("pet.get", "Pet not found");

            return pet;
        }

        public UnitResult<Error> AddPet(Pet pet)
        {
            if (_pets.Contains(pet))
                return Error.Failure("pet.add", "Failed adding pet");

            var position = Position.Create(_pets.Count + 1);

            pet.SetPosition(position.Value);

            _pets.Add(pet);

            return UnitResult.Success<Error>();
        }

        public UnitResult<Error> MovePet(Pet petToMove, Position newPosition)
        {
            if (newPosition.Value > _pets.Count)
                return Error.Failure("pet.move", "Failed moving pet");

            var orderPetsList = _pets.OrderBy(p => p.Position.Value).ToList();

            orderPetsList.Remove(petToMove);
            orderPetsList.Insert(newPosition.Value - 1, petToMove);

            RecountPositions(ref orderPetsList);

            return UnitResult.Success<Error>();
        }

        public UnitResult<Error> RemovePet(Pet pet)
        {
            if (_pets.Contains(pet) is false)
                return Error.Failure("pet.remove", "Failed remove pet");

            _pets.Remove(pet);
            
            var orderPetsList = _pets.OrderBy(p => p.Position.Value).ToList();
            
            RecountPositions(ref orderPetsList);

            return UnitResult.Success<Error>();
        }

        private static void RecountPositions(ref List<Pet> orderPetsList)
        {
            for (int i = 0; i < orderPetsList.Count; i++)
            {
                orderPetsList[i].SetPosition(Position.Create(i + 1).Value);
            }
        }
    }
}