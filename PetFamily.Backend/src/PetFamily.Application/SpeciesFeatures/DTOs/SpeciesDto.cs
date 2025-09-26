namespace PetFamily.Application.SpeciesFeatures.DTOs;

public record GetSpeciesDto(
    IEnumerable<SpeciesDto> Species);

public record SpeciesDto(
    Guid SpeciesId,
    string Name);