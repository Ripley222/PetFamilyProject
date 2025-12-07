namespace PetFamily.Application.VolunteersFeatures.PetFeatures.GetAll;

public record GetPetsQuery(
    Guid? VolunteerId,
    string? PetName,
    int? PetAge,
    string? SpeciesName,
    string? BreedsName,
    string? PetColor,
    string? PetCity,
    string? PetStreet,
    string? PetHouse,
    string? PetHealthInformation,
    string? PetHelpStatus,
    bool? IsNeutered,
    bool? IsVaccinated,
    int PageNumber = 1,
    int PageSize = 10);
