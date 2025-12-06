using PetFamily.Application.VolunteersFeatures.PetFeatures.GetAll;

namespace PetFamily.API.Requests.Pets;

public record GetPetsRequest(
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
    int PageSize = 10)
{
    public GetPetsQuery ToQuery()
    {
        return new GetPetsQuery(
            VolunteerId, 
            PetName,
            PetAge,
            SpeciesName,
            BreedsName,
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