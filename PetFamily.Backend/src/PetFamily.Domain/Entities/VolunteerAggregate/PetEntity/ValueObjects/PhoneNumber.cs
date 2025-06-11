using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects
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

        public static Result<PhoneNumber> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<PhoneNumber>("Телефон не заполнен либо есть лишние пробелы!");

            if (Regex.IsMatch(value, PHONE_REGEX) == false)
                return Result.Failure<PhoneNumber>("Неверный формат номера телефона!");

            if (value.Length > MAX_VALUE_LENGTH)
                return Result.Failure<PhoneNumber>($"Максимальная длина номера телефона - {MAX_VALUE_LENGTH} символов!");

            var phoneNumber = new PhoneNumber(value);

            return Result.Success(phoneNumber);
        }
    }
}
