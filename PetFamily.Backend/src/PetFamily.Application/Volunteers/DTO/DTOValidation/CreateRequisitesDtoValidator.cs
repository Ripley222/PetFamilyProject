using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;

namespace PetFamily.Application.Volunteers.DTO.DTOValidation;

public class CreateRequisitesDtoValidator : AbstractValidator<RequisitesDto>
{
    public CreateRequisitesDtoValidator()
    {
        RuleFor(c => new { c.AccountNumber, c.Title, c.Description })
            .MustBeValueObject(r => Requisite.Create(r.AccountNumber, r.Title, r.Description));
    }
}