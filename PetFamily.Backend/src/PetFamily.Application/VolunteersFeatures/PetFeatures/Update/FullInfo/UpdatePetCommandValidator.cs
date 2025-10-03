using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Application.VolunteersFeatures.DTOs.DTOsValidation;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.Update.FullInfo;

public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("VolunteerId"));
        
        RuleFor(u => u.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("PetId"));
        
        RuleFor(u => u.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("SpeciesId"));
        
        RuleFor(u => u.BreedId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("BreedId"));

        RuleFor(u => u.Name).MustBeValueObject(Name.Create);

        RuleFor(a => a.Description).MustBeValueObject(Description.Create);

        RuleFor(a => a.Color)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("Color"));

        RuleFor(a => a.HealthInformation).MustBeValueObject(HealthInformation.Create);

        RuleFor(a => new { a.City, a.Street, a.House })
            .MustBeValueObject(a => Address.Create(a.City, a.Street, a.House));

        RuleFor(a => new { a.Weight, a.Height })
            .MustBeValueObject(b => BodySize.Create(b.Weight, b.Height));

        RuleFor(a => a.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleFor(a => a.HelpStatus).MustBeValueObject(HelpStatus.Create);

        RuleForEach(a => a.Requisites).SetValidator(new CreateRequisitesDtoValidator());
    }
}