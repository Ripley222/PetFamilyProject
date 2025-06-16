using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

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

        public static Result<Address, Error> Create(string city, string street, string house)
        {
            if (string.IsNullOrWhiteSpace(city))
                return Errors.General.ValueIsRequired("City");
            
            if (city.Length > MAX_LENGTH_CITY)
                return Errors.General.ValueIsInvalid("City");

            if (string.IsNullOrWhiteSpace(street))
                return Errors.General.ValueIsRequired("Street");
            
            if (street.Length > MAX_LENGTH_STREET)
                return Errors.General.ValueIsInvalid("Street");

            if (string.IsNullOrWhiteSpace(house))
                return Errors.General.ValueIsRequired("House");
            
            if (house.Length > MAX_LENGTH_HOUSE)
                return Errors.General.ValueIsInvalid("House");

            return new Address(city, street, house);
        }
    }
}
