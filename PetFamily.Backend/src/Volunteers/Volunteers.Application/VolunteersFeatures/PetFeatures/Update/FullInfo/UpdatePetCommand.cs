using Application.Abstraction;
using Volunteers.Application.VolunteersFeatures.DTOs;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.Update.FullInfo;

public record UpdatePetCommand(
    Guid VolunteerId,
    Guid PetId,
    Guid SpeciesId,
    Guid BreedId,
    string Name,
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
    IEnumerable<RequisitesDto> Requisites) : ICommand;