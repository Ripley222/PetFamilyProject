using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects
{
    public record FullName
    {
        public const int MAX_FIRSTNAME_LENGTH = 200;
        public const int MAX_MIDDLENAME_LENGTH = 200;
        public const int MAX_LASTNAME_LENGTH = 200;
        
        public string FirstName { get; }
        public string MiddleName { get; }
        public string LastName { get; }

        private FullName(string firstName, string middleName, string lastName)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }

        public static Result<FullName, Error> Create(string firstName, string middleName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Errors.General.ValueIsRequired("FirstName");
            
            if (firstName.Length > Constants.MAX_LENGTH_NAME)
                return Errors.General.ValueIsInvalid("FirstName");

            if (string.IsNullOrWhiteSpace(middleName))
                return Errors.General.ValueIsRequired("MiddleName");
            
            if (middleName.Length > Constants.MAX_LENGTH_NAME)
                return Errors.General.ValueIsInvalid("MiddleName");

            if (string.IsNullOrWhiteSpace(lastName))
                return Errors.General.ValueIsRequired("LastName");
            
            if (lastName.Length > Constants.MAX_LENGTH_NAME)
                return Errors.General.ValueIsInvalid("LastName");

            return new FullName(firstName, middleName, lastName);
        }
    }
}
