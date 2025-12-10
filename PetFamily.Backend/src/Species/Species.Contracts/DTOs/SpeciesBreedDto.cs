namespace Species.Contracts.DTOs;

public record SpeciesBreedDto(Guid SpeciesId, Guid BreedId, string SpeciesName, string BreedName);