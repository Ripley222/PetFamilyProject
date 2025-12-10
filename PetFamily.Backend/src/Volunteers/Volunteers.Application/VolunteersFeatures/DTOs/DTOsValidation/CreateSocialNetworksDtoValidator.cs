using Application.Validation;
using FluentValidation;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.DTOs.DTOsValidation;

public class CreateSocialNetworksDtoValidator : AbstractValidator<SocialNetworksDto>
{
    public CreateSocialNetworksDtoValidator()
    {
        RuleFor(c => new { c.Title, c.Link })
            .MustBeValueObject(s => SocialNetwork.Create(s.Title, s.Link));
    }
}