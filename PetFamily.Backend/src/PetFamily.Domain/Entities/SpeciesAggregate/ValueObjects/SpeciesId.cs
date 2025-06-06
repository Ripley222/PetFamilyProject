using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects
{
    public class SpeciesId : ValueObject
    {
        public Guid Value { get; }

        private SpeciesId(Guid value)
        {
            Value = value;
        }

        public static Result<SpeciesId> Create(Guid id)
        {
            if (id == Guid.Empty)
                return Result.Failure<SpeciesId>("Отсутствует идентификационный номер вида животного!");

            var result = new SpeciesId(id);

            return Result.Success(result);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
