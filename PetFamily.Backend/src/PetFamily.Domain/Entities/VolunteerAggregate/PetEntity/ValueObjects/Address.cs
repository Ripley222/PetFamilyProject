using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects
{
    public record Address
    {
        public const int MAX_LENGTH_CITY = 50;
        public const int MAX_LENGTH_STREET = 50;
        public const int MAX_LENGTH_HOUSE = 10;
        
        public string City { get; }
        public string Street { get; }
        public string House { get; }

        private Address(string city, string street, string house)
        {
            City = city;
            Street = street;
            House = house;
        }

        public static Result<Address> Create(string city, string street, string house)
        {
            if (string.IsNullOrWhiteSpace(city))
                return Result.Failure<Address>("Необходимо указать город в котором находится животное!");
            
            if (city.Length > MAX_LENGTH_CITY)
                return Result.Failure<Address>($"Максимальная длина названия города - {MAX_LENGTH_CITY} символов!");

            if (string.IsNullOrWhiteSpace(street))
                return Result.Failure<Address>("Необходимо указать улицу на которой находится животное!");
            
            if (street.Length > MAX_LENGTH_STREET)
                return Result.Failure<Address>($"Максимальная длина названия улицы - {MAX_LENGTH_STREET} символов!");

            if (string.IsNullOrWhiteSpace(house))
                return Result.Failure<Address>("Необходимо указать дом в котором находится животное!");
            
            if (house.Length > MAX_LENGTH_HOUSE)
                return Result.Failure<Address>($"Максимальная длина названия дома - {MAX_LENGTH_HOUSE} символов!");

            var address = new Address(city, street, house);

            return Result.Success(address);
        }
    }
}
