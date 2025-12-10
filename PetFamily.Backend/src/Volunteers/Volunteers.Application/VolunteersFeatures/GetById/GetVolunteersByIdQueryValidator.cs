using Application.Validation;
using FluentValidation;
using SharedKernel;

namespace Volunteers.Application.VolunteersFeatures.GetById;

public class GetVolunteersByIdQueryValidator : AbstractValidator<GetVolunteersByIdQuery>
{
    public GetVolunteersByIdQueryValidator()
    {
        RuleFor(g => g.VolunteerId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "volunteer.id",
                "VolunteerId cannot be empty",
                "VolunteerId"));
    }
}