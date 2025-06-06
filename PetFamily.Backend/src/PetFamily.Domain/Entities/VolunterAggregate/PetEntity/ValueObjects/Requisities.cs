using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.VolunterAggregate.PetEntity.ValueObjects
{
    public class Requisities : ValueObject
    {     
        public string AccountNumber { get; }
        public string Title { get; }
        public string Description { get; }

        private Requisities(string accountNumber, string title, string description)
        {
            AccountNumber = accountNumber;
            Title = title;
            Description = description;
        }

        public static Result<Requisities> Create(string accountNumber, string title, string description)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return Result.Failure<Requisities>("Необходимо указать номер счета!");

            if (accountNumber.Length != 20)
                return Result.Failure<Requisities>("Номер счета должен содержать 20 символов!");

            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<Requisities>("Необходимо указать название реквизита!");

            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<Requisities>("Необходимо указать описание реквизита!");

            var result = new Requisities(accountNumber, title, description);

            return Result.Success(result);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AccountNumber;
            yield return Title;
            yield return Description;
        }
    }
}
