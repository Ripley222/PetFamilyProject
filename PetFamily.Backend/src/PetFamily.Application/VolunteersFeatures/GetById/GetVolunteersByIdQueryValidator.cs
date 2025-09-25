using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.GetById;

public class GetVolunteersByIdQueryValidator : AbstractValidator<GetVolunteersByIdQuery>
{
    public GetVolunteersByIdQueryValidator()
    {
        RuleFor(g => g.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}