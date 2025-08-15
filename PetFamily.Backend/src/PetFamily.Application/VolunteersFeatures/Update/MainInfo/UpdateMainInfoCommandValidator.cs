using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.Update.MainInfo;

public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => new { u.FirstName, u.MiddleName, u.LastName })
            .MustBeValueObject(f 
                => FullName.Create(f.FirstName, f.MiddleName, f.LastName));
        
        RuleFor(u => u.EmailAddress).MustBeValueObject(EmailAddress.Create);

        RuleFor(u => u.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(u => u.YearsOfExperience)
            .InclusiveBetween(Volunteer.MIN_EXPERIENCE, Volunteer.MAX_EXPERIENCE)
            .WithError(Error.Validation(
                Errors.General.ValueIsInvalid("YearsOfExperience").Code,
                Errors.General.ValueIsInvalid("YearsOfExperience").Message));
        
        RuleFor(u => u.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
    }
}