using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using SharedKernel;

namespace Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects
{
    public record PhoneNumber
    {
        public const int MAX_VALUE_LENGTH = 12;
        
        private const string PHONE_REGEX = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";

        public string Value { get; }

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public static Result<PhoneNumber, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.GeneralErrors.ValueIsRequired("PhoneNumber");

            if (Regex.IsMatch(value, PHONE_REGEX) == false)
                return Errors.GeneralErrors.ValueIsInvalid("PhoneNumber");

            if (value.Length > MAX_VALUE_LENGTH)
                return Errors.GeneralErrors.ValueIsInvalid("PhoneNumber");

            return new PhoneNumber(value);
        }
    }
}
