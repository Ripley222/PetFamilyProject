using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.GetById;

public class GetVolunteersByIdCommandValidator : AbstractValidator<GetVolunteersByIdCommand>
{
    public GetVolunteersByIdCommandValidator()
    {
        RuleFor(g => g.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}