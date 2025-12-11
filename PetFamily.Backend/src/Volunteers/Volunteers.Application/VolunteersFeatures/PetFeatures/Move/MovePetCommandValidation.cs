using Application.Validation;
using FluentValidation;
using SharedKernel;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.Move;

public class MovePetCommandValidation : AbstractValidator<MovePetCommand>
{
    public MovePetCommandValidation()
    {
        RuleFor(m => m.VolunteerId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "volunteer.id",
                "VolunteerId cannot be empty",
                "VolunteerId"));

        RuleFor(m => m.PetId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "pet.id",
                "PetId cannot be empty",
                "PetId"));

        RuleFor(m => m.NewPosition)
            .MustBeValueObject(Position.Create);
    }
}