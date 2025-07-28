using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Application.Volunteers.DTOs.DTOsValidation;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(c => new { c.FirstName, c.MiddleName, c.LastName })
            .MustBeValueObject(f => FullName.Create(f.FirstName, f.MiddleName, f.LastName));
        
        RuleFor(c => c.EmailAddress).MustBeValueObject(EmailAddress.Create);

        RuleFor(c => c.Description).MustBeValueObject(Description.Create);

        RuleFor(c => c.YearsOfExperience)
            .InclusiveBetween(Volunteer.MIN_EXPERIENCE, Volunteer.MAX_EXPERIENCE)
            .WithError(Error.Validation(
                Errors.General.ValueIsInvalid("YearsOfExperience").Code,
                Errors.General.ValueIsInvalid("YearsOfExperience").Message));

        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.Requisites).SetValidator(new CreateRequisitesDtoValidator());

        RuleForEach(c => c.SocialNetworks).SetValidator(new CreateSocialNetworksDtoValidator());
    }
}