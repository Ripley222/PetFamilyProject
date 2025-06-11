using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects
{
    public record Requisites
    {
        public const int LENGTH_ACCOUNT_NUMBER = 20;
        
        public string AccountNumber { get; }
        public string Title { get; }
        public string Description { get; }

        private Requisites(string accountNumber, string title, string description)
        {
            AccountNumber = accountNumber;
            Title = title;
            Description = description;
        }

        public static Result<Requisites> Create(string accountNumber, string title, string description)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return Result.Failure<Requisites>("Необходимо указать номер счета!");

            if (accountNumber.Length != LENGTH_ACCOUNT_NUMBER)
                return Result.Failure<Requisites>("Номер счета должен содержать 20 символов!");

            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<Requisites>("Необходимо указать название реквизита!");
            
            if (title.Length < Constants.MAX_LENGTH_TITLE ||  title.Length > Constants.MAX_LENGTH_TITLE)
                return Result.Failure<Requisites>($"Длина название должна составлять {Constants.MAX_LENGTH_TITLE}-{Constants.MAX_LENGTH_TITLE} символов!");

            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<Requisites>("Необходимо указать описание реквизита!");
            
            if (description.Length < Constants.MIN_LENGTH_DESCRIPTION ||  description.Length > Constants.MAX_LENGTH_DESCRIPTION)
                return Result.Failure<Requisites>($"Длина описания должна составлять {Constants.MIN_LENGTH_DESCRIPTION}-{Constants.MAX_LENGTH_DESCRIPTION} символов!");

            var requisites = new Requisites(accountNumber, title, description);

            return Result.Success(requisites);
        }
    }
}
