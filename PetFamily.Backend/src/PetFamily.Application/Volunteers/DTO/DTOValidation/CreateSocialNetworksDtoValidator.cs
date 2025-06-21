using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace PetFamily.Application.Volunteers.DTO.DTOValidation;

public class CreateSocialNetworksDtoValidator : AbstractValidator<SocialNetworksDto>
{
    public CreateSocialNetworksDtoValidator()
    {
        RuleFor(c => new { c.Title, c.Link })
            .MustBeValueObject(s => SocialNetwork.Create(s.Title, s.Link));
    }
}