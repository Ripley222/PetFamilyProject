using Application.Abstraction;
using Application.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Species.Contracts;
using Species.Contracts.DTOs;
using Volunteers.Domain.VolunteerAggregate.PetEntity;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.Add;

public class AddPetHandler(
    IVolunteersRepository volunteerRepository,
    ISpeciesContract speciesContract,
    IValidator<AddPetCommand> validator,
    ILogger<AddPetHandler> logger) : ICommandHandler<Guid, AddPetCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
           return validationResult.GetErrors();
        
        var volunteer = await volunteerRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var petId = PetId.New();
        var name = Name.Create(command.Name).Value;
        var description = Description.Create(command.Description).Value;
        var healthInformation = HealthInformation.Create(command.HealthInformation).Value;
        var address = Address.Create(command.City, command.Street, command.House).Value;
        var bodySize = BodySize.Create(command.Height, command.Weight).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var helpStatus = HelpStatus.Create(command.HelpStatus).Value;
        
        var requisites = command.Requisites
            .Select(r => Requisite.Create(r.AccountNumber, r.Title, r.Description).Value)
            .ToList();
        
        var speciesBreedResult = await speciesContract.CheckExistSpeciesBreedsByIds(
                new GetSpeciesBreedByIdsDto(command.SpeciesId, command.BreedId), 
                cancellationToken);

        if (speciesBreedResult.IsFailure)
            return speciesBreedResult.Error;
        
        var speciesBreed = new SpeciesBreed(command.SpeciesId, command.BreedId);
        
        var pet = Pet.Create(
            petId,
            name,
            speciesBreed,
            description,
            command.Color,
            healthInformation,
            address,
            bodySize,
            phoneNumber,
            command.IsNeutered,
            command.DateOfBirth,
            command.IsVaccinated,
            helpStatus,
            requisites);
        
        if (pet.IsFailure)
            return pet.Error.ToErrorList();
        
        var addPetToVolunteerResult = volunteer.Value.AddPet(pet.Value);
        if (addPetToVolunteerResult.IsFailure)
            return addPetToVolunteerResult.Error.ToErrorList();
        
        var saveResult = await volunteerRepository.Save(volunteer.Value, cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error.ToErrorList();
        
        logger.LogInformation("Added pet with id {petId} to volunteer with id {volunteerId}",
            petId.Value, command.VolunteerId);

        return pet.Value.Id.Value;
    }
}