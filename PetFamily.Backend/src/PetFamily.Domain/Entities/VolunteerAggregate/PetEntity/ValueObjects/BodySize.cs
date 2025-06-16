using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

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

        public static Result<BodySize, Error> Create(double weight, double height)
        {
            if (weight < MIN_WEIGHT || weight > MAX_WEIGHT)
                return Errors.General.ValueIsInvalid(nameof(weight));

            if (height < MIN_HEIGHT || height > MAX_HEIGHT)
                return Errors.General.ValueIsInvalid(nameof(height));

            return new BodySize(weight, height);
        }
    }
}
