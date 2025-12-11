using Application.Abstraction;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.GetAll;

public record GetPetsQuery(
    Guid? VolunteerId,
    Guid? SpeciesId,
    Guid? BreedId,
    string? PetName,
    int? PetAge,
    string? PetColor,
    string? PetCity,
    string? PetStreet,
    string? PetHouse,
    string? PetHealthInformation,
    string? PetHelpStatus,
    bool? IsNeutered,
    bool? IsVaccinated,
    int PageNumber = 1,
    int PageSize = 10) : IQuery;
