using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.SpeciesFeatures.BreedsFeatures.Delete;

public class DeleteBreedsCommandValidator : AbstractValidator<DeleteBreedsCommand>
{
    public DeleteBreedsCommandValidator()
    {
        RuleFor(d => d.BreedId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "breed.id",
                "BreedId cannot be empty",
                "BreedId"));
    }
}