using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects
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

        public static Result<EmailAddress> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<EmailAddress>("Необходимо указать электронный адрес волонтёра!");

            if (Regex.IsMatch(value, EMAIL_REGEX) == false)
                return Result.Failure<EmailAddress>("Неверный формат электронной почты!");
            
            if (value.Length > MAX_LENGTH_EMAIL)
                return Result.Failure<EmailAddress>($"Максимальная длина названия эл. почты должна быть {MAX_LENGTH_EMAIL} символов!");

            var emailAddress = new EmailAddress(value);

            return Result.Success(emailAddress); 
        }
    }
}
