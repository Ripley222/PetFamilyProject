using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Update;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Update.FullInfo;

namespace PetFamily.API.Requests.Volunteer.Pet;

public record UpdatePetRequest(Guid SpeciesId,
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
    public UpdatePetCommand ToCommand(Guid volunteerId, Guid petId) =>
        new(volunteerId,
            petId,
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