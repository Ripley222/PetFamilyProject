using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.SpeciesFeatures.Delete;

public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
{
    public DeleteSpeciesCommandValidator()
    {
        RuleFor(x => x.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("SpeciesId"));
    }   
}