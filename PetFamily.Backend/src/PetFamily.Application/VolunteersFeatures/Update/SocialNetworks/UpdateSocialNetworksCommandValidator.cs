using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Application.VolunteersFeatures.DTOs.DTOsValidation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.Update.SocialNetworks;

public class UpdateSocialNetworksCommandValidator : AbstractValidator<UpdateSocialNetworksCommand>
{
    public UpdateSocialNetworksCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleForEach(u => u.SocialNetworks)
            .SetValidator(new CreateSocialNetworksDtoValidator());
    }
}