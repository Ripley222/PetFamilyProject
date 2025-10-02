using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.Enums;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects
{
    public record HelpStatus
    {
        public const int MAX_VALUE_LENGTH = 50;
        
        public static readonly HelpStatus NeedsHelp = new(nameof(HelpStatusEnum.NeedsHelp));
        public static readonly HelpStatus LookingHome = new(nameof(HelpStatusEnum.LookingHome));
        public static readonly HelpStatus FoundHome = new(nameof(HelpStatusEnum.FoundHome));

        private static readonly HelpStatus[] _all = [NeedsHelp, LookingHome, FoundHome];

        public string Value { get; }

        private HelpStatus(string value)
        {
            Value = value;
        }

        public static Result<HelpStatus, Error> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Errors.General.ValueIsRequired("HelpStatus");

            var valueToLower = value.Trim().ToLower();

            if (_all.Any(h => h.Value.ToLower() == valueToLower) is false)
                return Errors.General.ValueIsInvalid("HelpStatus");

            return new HelpStatus(value);
        }
    }
}
