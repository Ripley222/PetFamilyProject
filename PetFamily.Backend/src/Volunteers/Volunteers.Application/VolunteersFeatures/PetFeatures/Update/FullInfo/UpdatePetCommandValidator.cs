using Application.Validation;
using FluentValidation;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures.DTOs.DTOsValidation;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.Update.FullInfo;

public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "volunteer.id",
                "VolunteerId cannot be empty",
                "VolunteerId"));

        RuleFor(u => u.PetId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "pet.id",
                "PetId cannot be empty",
                "PetId"));

        RuleFor(u => u.SpeciesId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "species.id",
                "SpeciesId cannot be empty",
                "SpeciesId"));

        RuleFor(u => u.BreedId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "breed.id",
                "BreedId cannot be empty",
                "BreedId"));

        RuleFor(u => u.Name)
            .MustBeValueObject(Name.Create);

        RuleFor(a => a.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(a => a.Color)
            .NotEmpty()
            .WithError(Errors.GeneralErrors.ValueIsRequired("Color"));

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