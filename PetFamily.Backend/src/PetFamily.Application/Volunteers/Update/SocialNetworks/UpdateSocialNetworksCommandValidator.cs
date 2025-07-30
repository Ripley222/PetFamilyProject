using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Application.Volunteers.DTOs.DTOsValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Update.SocialNetworks;

public class UpdateSocialNetworksCommandValidator : AbstractValidator<UpdateSocialNetworksCommand>
{
    public UpdateSocialNetworksCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleForEach(x => x.SocialNetworks)
            .SetValidator(new CreateSocialNetworksDtoValidator());
    }
}