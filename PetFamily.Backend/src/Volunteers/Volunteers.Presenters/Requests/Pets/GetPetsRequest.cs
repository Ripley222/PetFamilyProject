using Volunteers.Application.VolunteersFeatures.PetFeatures.GetAll;

namespace Volunteers.Presenters.Requests.Pets;

public record GetPetsRequest(
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
    int PageSize = 10)
{
    public GetPetsQuery ToQuery()
    {
        return new GetPetsQuery(
            VolunteerId,
            SpeciesId,
            BreedId,
            PetName,
            PetAge,
            PetColor,
            PetCity,
            PetStreet,
            PetHouse,
            PetHealthInformation,
            PetHelpStatus,
            IsNeutered,
            IsVaccinated,
            PageNumber,
            PageSize);
    }
}