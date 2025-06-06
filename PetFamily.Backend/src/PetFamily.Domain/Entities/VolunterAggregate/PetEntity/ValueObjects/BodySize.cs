using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.VolunterAggregate.PetEntity.ValueObjects
{
    public class BodySize : ValueObject
    {
        public double Weight { get; }
        public double Height { get; }

        private BodySize(double weight, double height)
        {
            Weight = weight;
            Height = height;
        }

        public static Result<BodySize> Create(double weight, double height)
        {
            if (weight <= 0 || weight > 150)
                return Result.Failure<BodySize>("Вес животного должен быть в пределах 1-150 кг!");

            if (height <= 0 || height > 300)
                return Result.Failure<BodySize>("Рост животного должен быть в пределах 1-300см");

            var result = new BodySize(weight, height); 

            return Result.Success(result);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Weight;
            yield return Height;
        }
    }
}
