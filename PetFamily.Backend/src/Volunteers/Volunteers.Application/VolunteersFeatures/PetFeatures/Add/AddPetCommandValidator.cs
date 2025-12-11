using Application.Validation;
using FluentValidation;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures.DTOs.DTOsValidation;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.Add;

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
                Errors.GeneralErrors.ValueIsInvalid("Color").Code,
                Errors.GeneralErrors.ValueIsInvalid("Color").Message));

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