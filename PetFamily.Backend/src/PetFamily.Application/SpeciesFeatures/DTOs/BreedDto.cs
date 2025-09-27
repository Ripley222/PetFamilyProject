namespace PetFamily.Application.SpeciesFeatures.DTOs;

public record GetBreedsDto(
    IEnumerable<BreedDto> Breeds);

public record BreedDto(
    Guid BreedId,
    string Name);