using CSharpFunctionalExtensions;
using SharedKernel;

namespace Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects
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
                return Errors.GeneralErrors.ValueIsRequired("City");
            
            if (city.Length > MAX_LENGTH_CITY)
                return Errors.GeneralErrors.ValueIsInvalid("City");

            if (string.IsNullOrWhiteSpace(street))
                return Errors.GeneralErrors.ValueIsRequired("Street");
            
            if (street.Length > MAX_LENGTH_STREET)
                return Errors.GeneralErrors.ValueIsInvalid("Street");

            if (string.IsNullOrWhiteSpace(house))
                return Errors.GeneralErrors.ValueIsRequired("House");
            
            if (house.Length > MAX_LENGTH_HOUSE)
                return Errors.GeneralErrors.ValueIsInvalid("House");

            return new Address(city, street, house);
        }
    }
}
