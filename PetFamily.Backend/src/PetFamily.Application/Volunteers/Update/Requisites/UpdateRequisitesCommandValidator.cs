using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Application.Volunteers.DTOs.DTOsValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.Requisites;

public class UpdateRequisitesCommandValidator : AbstractValidator<UpdateRequisitesCommand>
{
    public UpdateRequisitesCommandValidator()
    {
        RuleFor(x => x.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(x => x.Requisites)
            .SetValidator(new CreateRequisitesDtoValidator());
    }
}