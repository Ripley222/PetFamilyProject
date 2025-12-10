using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using SharedKernel;

namespace Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects
{
    public record EmailAddress
    {
        public const int MAX_LENGTH_EMAIL = 100;
        
        private const string EMAIL_REGEX = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        public string Value { get; }

        private EmailAddress(string value)
        {
             Value = value;
        }        

        public static Result<EmailAddress, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.GeneralErrors.ValueIsRequired("EmailAddress");

            if (Regex.IsMatch(value, EMAIL_REGEX) == false)
                return Errors.GeneralErrors.ValueIsInvalid("EmailAddress");
            
            if (value.Length > MAX_LENGTH_EMAIL)
                return Errors.GeneralErrors.ValueIsInvalid("EmailAddress");

            return new EmailAddress(value);
        }
    }
}
