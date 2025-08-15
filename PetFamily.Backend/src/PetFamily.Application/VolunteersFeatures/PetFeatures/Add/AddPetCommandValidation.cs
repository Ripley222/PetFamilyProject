using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Application.VolunteersFeatures.DTOs.DTOsValidation;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.Add;

public class AddPetCommandValidation :  AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidation()
    {
        RuleFor(a => a.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());

        RuleFor(a => a.SpeciesName).NotEmpty().WithError(Errors.General.ValueIsRequired());

        RuleFor(a => a.BreedName).NotEmpty().WithError(Errors.General.ValueIsRequired());

        RuleFor(a => a.Name).MustBeValueObject(Name.Create);

        RuleFor(a => a.Description).MustBeValueObject(Description.Create);

        RuleFor(a => a.Color)
            .NotEmpty()
            .WithError(Error.Validation(
                Errors.General.ValueIsInvalid("Color").Code,
                Errors.General.ValueIsInvalid("Color").Message));

        RuleFor(a => a.HealthInformation).MustBeValueObject(HealthInformation.Create);

        RuleFor(a => new { a.City, a.Street, a.House })
            .MustBeValueObject(a => Address.Create(a.City, a.Street, a.House));

        RuleFor(a => new { a.Weight, a.Height })
            .MustBeValueObject(b => BodySize.Create(b.Weight, b.Height));
        
        RuleFor(a => a.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleFor(a => a.HelpStatus).MustBeValueObject(HelpStatus.Create);
        
        RuleForEach(a => a.Requisites)
            .SetValidator(new CreateRequisitesDtoValidator());
    }
}