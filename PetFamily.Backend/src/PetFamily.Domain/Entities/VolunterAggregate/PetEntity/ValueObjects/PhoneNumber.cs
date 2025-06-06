using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace PetFamily.Domain.Entities.VolunterAggregate.PetEntity.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        private const string phoneRegex = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";

        public string Value { get; }

        private PhoneNumber(string number)
        {
            Value = number;
        }

        public static Result<PhoneNumber> Create(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return Result.Failure<PhoneNumber>("Телефон не заполнен либо есть лишние пробелы!");

            if (Regex.IsMatch(number, phoneRegex) == false)
                return Result.Failure<PhoneNumber>("Неверный формат номера телефона!");

            var result = new PhoneNumber(number);

            return Result.Success(result);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
