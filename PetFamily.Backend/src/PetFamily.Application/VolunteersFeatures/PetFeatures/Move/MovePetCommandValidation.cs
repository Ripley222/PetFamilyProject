using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.Move;

public class MovePetCommandValidation : AbstractValidator<MovePetCommand>
{
    public MovePetCommandValidation()
    {
        RuleFor(m => m.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("VolunteerId"));

        RuleFor(m => m.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("PetId"));

        RuleFor(m => m.NewPosition)
            .MustBeValueObject(Position.Create);
    }
}