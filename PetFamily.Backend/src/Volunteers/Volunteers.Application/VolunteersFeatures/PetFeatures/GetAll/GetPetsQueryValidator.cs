using Application.Validation;
using FluentValidation;
using SharedKernel;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.GetAll;

public class GetPetsQueryValidator : AbstractValidator<GetPetsQuery>
{
    public GetPetsQueryValidator()
    {
        RuleFor(g => g.VolunteerId)
            .Must(i => i != Guid.Empty)
            .When(g => g.VolunteerId is not null)
            .WithError(Error.Validation(
                "volunteer.id",
                "Volunteer id must not be empty",
                "VolunteerId"));

        RuleFor(g => g.PetAge)
            .InclusiveBetween(0, 100)
            .WithError(Error.Validation(
                "pet.age",
                "Must be between 0 and 100",
                "PetAge"));

        RuleFor(g => g.PageNumber)
            .Must(p => p > 0)
            .WithError(Error.Validation(
                "page.validation",
                "Page must be greater than zero",
                "PageNumber"));

        RuleFor(g => g.PageSize)
            .Must(ps => ps is > 0 and <= 20)
            .WithError(Error.Validation(
                "page.size.validation",
                "Page size must be greater than zero and less than thirty",
                "PageSize"));
    }
}