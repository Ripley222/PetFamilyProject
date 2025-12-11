using CSharpFunctionalExtensions;
using SharedKernel;

namespace Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects
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
                return Errors.GeneralErrors.ValueIsRequired("FirstName");
            
            if (firstName.Length > Constants.MAX_LENGTH_NAME)
                return Errors.GeneralErrors.ValueIsInvalid("FirstName");

            if (string.IsNullOrWhiteSpace(middleName))
                return Errors.GeneralErrors.ValueIsRequired("MiddleName");
            
            if (middleName.Length > Constants.MAX_LENGTH_NAME)
                return Errors.GeneralErrors.ValueIsInvalid("MiddleName");

            if (string.IsNullOrWhiteSpace(lastName))
                return Errors.GeneralErrors.ValueIsRequired("LastName");
            
            if (lastName.Length > Constants.MAX_LENGTH_NAME)
                return Errors.GeneralErrors.ValueIsInvalid("LastName");

            return new FullName(firstName, middleName, lastName);
        }
    }
}
