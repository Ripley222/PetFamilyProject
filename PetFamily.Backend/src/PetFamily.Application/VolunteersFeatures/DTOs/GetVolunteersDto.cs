namespace PetFamily.Application.VolunteersFeatures.DTOs;

public record GetVolunteersDto(
    IEnumerable<VolunteerDto> Volunteers);