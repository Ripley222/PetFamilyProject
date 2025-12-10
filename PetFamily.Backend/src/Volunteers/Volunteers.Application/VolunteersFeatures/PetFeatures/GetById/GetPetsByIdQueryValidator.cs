using Application.Validation;
using FluentValidation;
using SharedKernel;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.GetById;

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