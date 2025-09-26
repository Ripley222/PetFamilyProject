using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace PetFamily.Application.VolunteersFeatures.DTOs;

public record VolunteerDto(
    Guid VolunteerId,
    string FirstName,
    string MiddleName,
    string LastName,
    string Description,
    int YearsOfExperience,
    string PhoneNumber,
    IEnumerable<Requisite> Requisites,
    IEnumerable<SocialNetwork> Socials);