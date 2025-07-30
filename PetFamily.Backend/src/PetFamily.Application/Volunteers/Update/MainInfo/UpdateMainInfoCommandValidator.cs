using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.MainInfo;

public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoCommandValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(x => new { x.FirstName, x.MiddleName, x.LastName })
            .MustBeValueObject(f 
                => FullName.Create(f.FirstName, f.MiddleName, f.LastName));
        
        RuleFor(c => c.EmailAddress).MustBeValueObject(EmailAddress.Create);

        RuleFor(x => x.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(x => x.YearsOfExperience)
            .InclusiveBetween(Volunteer.MIN_EXPERIENCE, Volunteer.MAX_EXPERIENCE)
            .WithError(Error.Validation(
                Errors.General.ValueIsInvalid("YearsOfExperience").Code,
                Errors.General.ValueIsInvalid("YearsOfExperience").Message));
        
        RuleFor(x => x.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
    }
}