﻿using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Application.VolunteersFeatures.PetFeatures.Add;

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
    IEnumerable<RequisitesDto> Requisites)
{
    public AddPetCommand ToCommand(Guid volunteerId) =>
        new(volunteerId,
            SpeciesName,
            BreedName,
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