using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects
{
    public record Requisites
    {
        public const int LENGTH_ACCOUNT_NUMBER = 20;
        
        public string AccountNumber { get; } = string.Empty;
        public string Title { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        
        private Requisites(string accountNumber, string title, string description)
        {
            AccountNumber = accountNumber;
            Title = title;
            Description = description;
        }

        public static Result<Requisites, Error> Create(string accountNumber, string title, string description)
        {
            if (accountNumber.Length != 0 && accountNumber.Length != LENGTH_ACCOUNT_NUMBER)
                return Errors.General.ValueIsInvalid("AccountNumber");
            
            if (title.Length > Constants.MAX_LENGTH_TITLE)
                return Errors.General.ValueIsInvalid("Title");
            
            if (description.Length > Constants.MAX_LENGTH_DESCRIPTION)
                return Errors.General.ValueIsInvalid("Description");

            return new Requisites(accountNumber, title, description);
        }
    }
}
