using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace PetFamily.Domain.Entities.VolunterAggregate.VolunteerEntity.ValueObjects
{
    public class EmailAddress : ValueObject
    {
        private const string emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        public string Value { get; }

        private EmailAddress(string value)
        {
             Value = value;
        }        

        public static Result<EmailAddress> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<EmailAddress>("Необходимо указать электронный адрес волонтёра!");

            if (Regex.IsMatch(value, emailRegex) == false)
                return Result.Failure<EmailAddress>("Неверный формат электронной почты!");

            var result = new EmailAddress(value);

            return Result.Success(result); 
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
