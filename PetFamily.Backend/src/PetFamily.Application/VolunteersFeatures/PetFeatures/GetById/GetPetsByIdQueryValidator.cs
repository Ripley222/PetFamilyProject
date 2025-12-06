using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.PetFeatures.GetById;

public class GetPetsByIdQueryValidator : AbstractValidator<GetPetsByIdQuery>
{
    public GetPetsByIdQueryValidator()
    {
        RuleFor(g => g.PetId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "pet.id",
                "PetId cannot be empty",
                "PetId"));
    }
}