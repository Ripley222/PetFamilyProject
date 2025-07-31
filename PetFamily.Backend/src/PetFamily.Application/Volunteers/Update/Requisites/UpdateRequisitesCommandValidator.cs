using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Application.Volunteers.DTOs.DTOsValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.Requisites;

public class UpdateRequisitesCommandValidator : AbstractValidator<UpdateRequisitesCommand>
{
    public UpdateRequisitesCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(u => u.Requisites)
            .SetValidator(new CreateRequisitesDtoValidator());
    }
}