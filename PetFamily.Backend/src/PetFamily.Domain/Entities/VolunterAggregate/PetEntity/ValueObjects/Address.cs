using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.VolunterAggregate.PetEntity.ValueObjects
{
    public class Address : ValueObject
    {
        public string Country { get; }
        public string City { get; }
        public string Street { get; }
        public string House { get; }

        private Address(string country, string city, string street, string house)
        {
            Country = country;
            City = city;
            Street = street;
            House = house;
        }

        public static Result<Address> Create(string country, string city, string street, string house)
        {
            if (string.IsNullOrWhiteSpace(city))
                return Result.Failure<Address>("Необходимо указать город в котором находится животное!");

            if (string.IsNullOrWhiteSpace(street))
                return Result.Failure<Address>("Необходимо указать улицу на которой находится животное!");

            if (string.IsNullOrWhiteSpace(house))
                return Result.Failure<Address>("Необходимо указать дом в котором находится животное!");

            var result = new Address(country, city, street, house);

            return Result.Success(result);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Country;
            yield return City;
            yield return Street;
            yield return House;
        }
    }
}
