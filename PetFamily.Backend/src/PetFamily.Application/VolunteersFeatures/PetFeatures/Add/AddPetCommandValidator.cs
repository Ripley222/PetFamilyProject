using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Application.VolunteersFeatures.DTOs.DTOsValidation;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.Add;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        RuleFor(a => a.VolunteerId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "volunteer.id",
                "VolunteerId cannot be empty",
                "VolunteerId"));

        RuleFor(a => a.SpeciesId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "species.id",
                "SpeciesId cannot be empty",
                "SpeciesId"));

        RuleFor(a => a.BreedId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "breed.id",
                "BreedId cannot be empty",
                "BreedId"));

        RuleFor(a => a.Name)
            .MustBeValueObject(Name.Create);

        RuleFor(a => a.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(a => a.Color)
            .NotEmpty()
            .WithError(Error.Validation(
                Errors.General.ValueIsInvalid("Color").Code,
                Errors.General.ValueIsInvalid("Color").Message));

        RuleFor(a => a.HealthInformation)
            .MustBeValueObject(HealthInformation.Create);

        RuleFor(a => new { a.City, a.Street, a.House })
            .MustBeValueObject(a => Address.Create(a.City, a.Street, a.House));

        RuleFor(a => new { a.Weight, a.Height })
            .MustBeValueObject(b => BodySize.Create(b.Weight, b.Height));

        RuleFor(a => a.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);

        RuleFor(a => a.HelpStatus)
            .MustBeValueObject(HelpStatus.Create);

        RuleForEach(a => a.Requisites)
            .SetValidator(new CreateRequisitesDtoValidator());
    }
}