using Application.Abstraction;
using Application.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Species.Contracts;
using Species.Contracts.DTOs;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.Update.FullInfo;

public class UpdatePetHandler(
    IVolunteersRepository volunteersRepository,
    ISpeciesContract speciesContract,
    IValidator<UpdatePetCommand> validator,
    ILogger<UpdatePetHandler> logger) : ICommandHandler<Guid, UpdatePetCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid is false)
            return validationResult.GetErrors();

        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteerExistResult = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerExistResult.IsFailure)
            return volunteerExistResult.Error.ToErrorList();

        var speciesBreedExistResult = await speciesContract.CheckExistSpeciesBreedsByIds(
            new GetSpeciesBreedByIdsDto(command.SpeciesId, command.BreedId),
            cancellationToken);

        if (speciesBreedExistResult.IsFailure)
            return speciesBreedExistResult.Error;

        var petId = PetId.Create(command.PetId);

        var petResult = volunteerExistResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var petName = Name.Create(command.Name).Value;
        var speciesBreed = new SpeciesBreed(command.SpeciesId, command.BreedId);
        var description = Description.Create(command.Description).Value;
        var healthInfo = HealthInformation.Create(command.HealthInformation).Value;
        var address = Address.Create(command.City, command.Street, command.House).Value;
        var bodySize = BodySize.Create(command.Weight, command.Height).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var helpStatus = HelpStatus.Create(command.HelpStatus).Value;

        var requisites = command.Requisites
            .Select(r => Requisite.Create(r.AccountNumber, r.Title, r.Description).Value)
            .ToList();

        var updateResult = petResult.Value.Update(
            petName,
            speciesBreed,
            description,
            command.Color,
            healthInfo,
            address,
            bodySize,
            phoneNumber,
            command.IsNeutered,
            command.DateOfBirth,
            command.IsVaccinated,
            helpStatus,
            requisites);

        if (updateResult.IsFailure)
            return updateResult.Error.ToErrorList();

        var saveResult = await volunteersRepository.Save(volunteerExistResult.Value, cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error.ToErrorList();

        logger.LogInformation("Updated pet with id {petId}", petId.Value);

        return petId.Value;
    }
}