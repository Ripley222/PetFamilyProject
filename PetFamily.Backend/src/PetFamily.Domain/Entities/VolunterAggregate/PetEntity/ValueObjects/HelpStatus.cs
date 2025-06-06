using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.VolunterAggregate.PetEntity.ValueObjects
{
    public class HelpStatus : ValueObject
    {
        public static readonly HelpStatus NeedsHelp = new(nameof(NeedsHelp));
        public static readonly HelpStatus LookingHome = new(nameof(LookingHome));
        public static readonly HelpStatus FoundHome = new(nameof(FoundHome));

        private static readonly HelpStatus[] _all = [NeedsHelp, LookingHome, FoundHome];

        public string Value { get; }

        private HelpStatus(string value)
        {
            Value = value;
        }

        public static Result<HelpStatus> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<HelpStatus>("Необходимо указать статус помощи животному!");

            var helpStatus = value.Trim().ToLower();

            if (_all.Any(h => h.Value.ToLower() == helpStatus) == false)
                return Result.Failure<HelpStatus>("Некорректный статус помощи животному!");

            var result = new HelpStatus(helpStatus);

            return Result.Success(result);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
