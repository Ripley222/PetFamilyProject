using PetFamily.Application.VolunteersFeatures.DTOs;

namespace PetFamily.API.Requests.Volunteer.Pet;

public record AddPetRequest(
    string SpeciesName,
    string BreedName,
    string PetName,
    string Description,
    string Color,
    string HealthInformation,
    string City,
    string Street,
    string House,
    double Weight,
    double Height,
    string PhoneNumber,
    bool IsNeutered,
    DateOnly DateOfBirth,
    bool IsVaccinated,
    string HelpStatus,
    IEnumerable<RequisitesDto> Requisites);