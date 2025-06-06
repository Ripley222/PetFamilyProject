using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.VolunterAggregate.VolunteerEntity.ValueObjects
{
    public class FullName : ValueObject
    {
        public string FirstName { get; }
        public string MiddleName { get; }
        public string LastName { get; }

        private FullName(string firstName, string middleName, string lastName)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }

        public static Result<FullName> Create(string firstName, string middleName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Result.Failure<FullName>("Необходимо указать имя волонтера!");

            if (string.IsNullOrWhiteSpace(middleName))
                return Result.Failure<FullName>("Необходимо указать фамилию волонтера!");

            if (string.IsNullOrWhiteSpace(lastName))
                return Result.Failure<FullName>("Необходимо указать отчество волонтера!");

            var result = new FullName(firstName, middleName, lastName);

            return Result.Success(result);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return MiddleName;
            yield return LastName;
        }
    }
}
