using Application.Validation;
using FluentValidation;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.DTOs.DTOsValidation;

public class CreateRequisitesDtoValidator : AbstractValidator<RequisitesDto>
{
    public CreateRequisitesDtoValidator()
    {
        RuleFor(c => new { c.AccountNumber, c.Title, c.Description })
            .MustBeValueObject(r => Requisite.Create(r.AccountNumber, r.Title, r.Description));
    }
}