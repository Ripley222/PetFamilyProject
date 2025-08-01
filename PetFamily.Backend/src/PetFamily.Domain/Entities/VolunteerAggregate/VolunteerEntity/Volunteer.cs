﻿using CSharpFunctionalExtensions;
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
            List<SocialNetwork> socials)  : base(id)
        {
            FullName = fullName;
            EmailAddress = emailAddress;
            Description = description;
            YearsOfExperience = yearsOfExperience;
            PhoneNumber = phoneNumber;
            Requisites = requisites;
            Socials = socials;
        }

        public FullName FullName { get; private set; }
        public EmailAddress EmailAddress { get; private set; }
        public Description Description { get; private set; }
        public int YearsOfExperience { get; private set; }        
        public PhoneNumber PhoneNumber { get; private set; }
        public List<Requisite> Requisites { get; private set; }
        public List<SocialNetwork> Socials { get; private set; }
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

        public Result AddPet(Pet pet)
        {
            if (_pets.Contains(pet))
                return Result.Failure("Указанное животное уже числится за волонтёром!");

            _pets.Add(pet);

            return Result.Success();
        }
    }
}
