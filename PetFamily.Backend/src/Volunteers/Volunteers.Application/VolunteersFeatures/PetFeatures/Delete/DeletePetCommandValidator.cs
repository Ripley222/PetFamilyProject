using Application.Validation;
using FluentValidation;
using SharedKernel;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.Delete;

public class DeletePetCommandValidator : AbstractValidator<DeletePetCommand>
{
    public DeletePetCommandValidator()
    {
        RuleFor(d => d.VolunteerId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "volunteer.id",
                "VolunteerId cannot be empty",
                "VolunteerId"));
        
        RuleFor(d => d.PetId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "pet.id",
                "PetId cannot be empty",
                "PetId"));
    }
}