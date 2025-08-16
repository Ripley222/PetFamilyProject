using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.SpeciesFeatures;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.Add;

public class AddPetHandler(
    IVolunteersRepository volunteerRepository,
    ISpeciesRepository speciesRepository,
    IValidator<AddPetCommand> validator,
    ILogger<AddPetHandler> logger)
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
        
        var speciesExistResult = await speciesRepository
            .GetByName(command.SpeciesName, cancellationToken);
        
        if (speciesExistResult.IsFailure)
            return speciesExistResult.Error.ToErrorList();

        var breedExist = speciesExistResult.Value.Breeds
            .FirstOrDefault(b => b.Name == command.BreedName);
        
        if (breedExist is null)
            return Errors.Breed.NotFound().ToErrorList();

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
        
        var speciesBreed = SpeciesBreed.Create(
            SpeciesId.Create(speciesExistResult.Value.Id.Value),
            BreedId.Create(breedExist.Id.Value));
        
        var pet = Pet.Create(
            petId,
            name,
            speciesBreed.Value,
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
        
        var result = volunteer.Value.AddPet(pet.Value);
        if (result.IsFailure)
            return result.Error.ToErrorList();
        
        await volunteerRepository.Save(volunteer.Value, cancellationToken);
        
        logger.LogInformation("Added pet with id {petId} to volunteer with id {volunteerId}",
            petId.Value, command.VolunteerId);

        return result.Value;
    }
}