using Application.Validation;
using FluentValidation;
using SharedKernel;

namespace Volunteers.Application.VolunteersFeatures.PetFeatures.PetFiles.GetLink;

public class GetPetFileLinkCommandValidation : AbstractValidator<GetPetFileLinkCommand>
{
    public GetPetFileLinkCommandValidation()
    {
        RuleFor(a => a.VolunteerId)
            .NotEmpty()
            .WithError(Errors.GeneralErrors.ValueIsRequired("VolunteerId"));
        
        RuleFor(a => a.PetId)
            .NotEmpty()
            .WithError(Errors.GeneralErrors.ValueIsRequired("PetId"));
        
        RuleFor(a => a.FileName)
            .NotEmpty()
            .WithError(Errors.GeneralErrors.ValueIsRequired("FileName"));
    }
}