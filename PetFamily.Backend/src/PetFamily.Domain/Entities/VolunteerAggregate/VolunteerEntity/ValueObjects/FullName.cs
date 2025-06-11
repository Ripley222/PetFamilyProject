using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects
{
    public record FullName
    {
        public const int MAX_FIRSTNAME_LENGTH = 200;
        public const int MAX_MIDDLENAME_LENGTH = 200;
        public const int MAX_LASTNAME_LENGTH = 200;
        
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
            
            if (firstName.Length < Constants.MIN_LENGTH_NAME || firstName.Length > Constants.MAX_LENGTH_NAME)
                return Result.Failure<FullName>("Имя должно состоять из 2-100 символов!");

            if (string.IsNullOrWhiteSpace(middleName))
                return Result.Failure<FullName>("Необходимо указать фамилию волонтера!");
            
            if (middleName.Length < Constants.MIN_LENGTH_NAME || middleName.Length > Constants.MAX_LENGTH_NAME)
                return Result.Failure<FullName>("Фамилия должна состоять из 2-100 символов!");

            if (string.IsNullOrWhiteSpace(lastName))
                return Result.Failure<FullName>("Необходимо указать отчество волонтера!");
            
            if (lastName.Length < Constants.MIN_LENGTH_NAME || lastName.Length > Constants.MAX_LENGTH_NAME)
                return Result.Failure<FullName>("Отчество должно состоять из 2-100 символов!");

            var fullName = new FullName(firstName, middleName, lastName);

            return Result.Success(fullName);
        }
    }
}
