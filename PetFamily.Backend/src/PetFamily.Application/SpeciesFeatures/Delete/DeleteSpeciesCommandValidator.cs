using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.SpeciesFeatures.Delete;

public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
{
    public DeleteSpeciesCommandValidator()
    {
        RuleFor(x => x.SpeciesId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "species.id",
                "SpeciesId cannot be empty",
                "SpeciesId"));
    }   
}