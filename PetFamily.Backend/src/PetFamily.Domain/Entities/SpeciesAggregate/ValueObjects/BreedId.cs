using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects
{
    public class BreedId : ValueObject
    {
        public Guid Value { get; }

        private BreedId(Guid value)
        {
            Value = value;
        }

        public static Result<BreedId> Create(Guid id)
        {
            if (id == Guid.Empty)
                return Result.Failure<BreedId>("Отсутствует идентификационный номер породы животного!");

            var result = new BreedId(id);

            return Result.Success(result);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
