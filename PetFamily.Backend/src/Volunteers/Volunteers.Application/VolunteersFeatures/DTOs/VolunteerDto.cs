namespace Volunteers.Application.VolunteersFeatures.DTOs;

public record VolunteerDto(
    Guid VolunteerId,
    string FirstName,
    string MiddleName,
    string LastName,
    string Description,
    int YearsOfExperience,
    string PhoneNumber,
    IEnumerable<RequisitesDto> Requisites,
    IEnumerable<SocialNetworksDto> Socials);