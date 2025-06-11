using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects
{
    public record BodySize
    {
        public const int MIN_WEIGHT = 1;
        public const int MIN_HEIGHT = 1;
        public const int MAX_WEIGHT = 150;
        public const int MAX_HEIGHT = 300;
        
        public double Weight { get; }
        public double Height { get; }

        private BodySize(double weight, double height)
        {
            Weight = weight;
            Height = height;
        }

        public static Result<BodySize> Create(double weight, double height)
        {
            if (weight < MAX_WEIGHT || weight > MAX_WEIGHT)
                return Result.Failure<BodySize>($"Вес животного должен быть в пределах {MIN_WEIGHT}-{MAX_WEIGHT}кг!");

            if (height < MIN_HEIGHT || height > MAX_HEIGHT)
                return Result.Failure<BodySize>($"Рост животного должен быть в пределах {MIN_HEIGHT}-{MAX_HEIGHT}см");

            var bodySize = new BodySize(weight, height); 

            return Result.Success(bodySize);
        }
    }
}
