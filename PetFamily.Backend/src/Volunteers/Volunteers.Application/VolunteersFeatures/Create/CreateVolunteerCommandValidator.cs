using Application.Validation;
using FluentValidation;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures.DTOs.DTOsValidation;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.Create;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(c => new { c.FirstName, c.MiddleName, c.LastName })
            .MustBeValueObject(f 
                => FullName.Create(f.FirstName, f.MiddleName, f.LastName));
        
        RuleFor(c => c.EmailAddress)
            .MustBeValueObject(EmailAddress.Create);

        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(c => c.YearsOfExperience)
            .InclusiveBetween(Volunteer.MIN_EXPERIENCE, Volunteer.MAX_EXPERIENCE)
            .WithError(Error.Validation(
                Errors.GeneralErrors.ValueIsInvalid("YearsOfExperience").Code,
                Errors.GeneralErrors.ValueIsInvalid("YearsOfExperience").Message));

        RuleFor(c => c.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.Requisites)
            .SetValidator(new CreateRequisitesDtoValidator());

        RuleForEach(c => c.SocialNetworks)
            .SetValidator(new CreateSocialNetworksDtoValidator());
    }
}