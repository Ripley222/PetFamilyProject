using Volunteers.Application.VolunteersFeatures.DTOs;
using Volunteers.Application.VolunteersFeatures.PetFeatures.Add;

namespace Volunteers.Presenters.Requests.Volunteer.Pet;

public record AddPetRequest(
    Guid SpeciesId,
    Guid BreedId,
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
    IEnumerable<RequisitesDto> Requisites)
{
    public AddPetCommand ToCommand(Guid volunteerId) =>
        new(volunteerId,
            SpeciesId,
            BreedId,
            PetName,
            Description,
            Color,
            HealthInformation,
            City,
            Street,
            House,
            Weight,
            Height,
            PhoneNumber,
            IsNeutered,
            DateOfBirth,
            IsVaccinated,
            HelpStatus,
            Requisites);
}