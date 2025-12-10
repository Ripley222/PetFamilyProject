using Application.Validation;
using FluentValidation;
using SharedKernel;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.Update.MainInfo;

public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .Must(i => i != Guid.Empty)
            .WithError(Error.Validation(
                "volunteer.id",
                "VolunteerId cannot be empty",
                "VolunteerId"));

        RuleFor(u => new { u.FirstName, u.MiddleName, u.LastName })
            .MustBeValueObject(f
                => FullName.Create(f.FirstName, f.MiddleName, f.LastName));

        RuleFor(u => u.EmailAddress)
            .MustBeValueObject(EmailAddress.Create);

        RuleFor(u => u.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(u => u.YearsOfExperience)
            .InclusiveBetween(Volunteer.MIN_EXPERIENCE, Volunteer.MAX_EXPERIENCE)
            .WithError(Error.Validation(
                Errors.GeneralErrors.ValueIsInvalid("YearsOfExperience").Code,
                Errors.GeneralErrors.ValueIsInvalid("YearsOfExperience").Message));

        RuleFor(u => u.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
    }
}