using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.GetWithPagination;

public class GetVolunteersQueryValidator : AbstractValidator<GetVolunteersQuery>
{
    public GetVolunteersQueryValidator()
    {
        RuleFor(g => g.Page)
            .Must(p => p > 0)
            .WithError(Error.Validation(
                "page.validation", 
                "Page must be greater than zero", 
                "Page"));
        
        RuleFor(g => g.PageSize)
            .Must(ps => ps is > 0 and <= 20)
            .WithError(Error.Validation(
                "page.size.validation", 
                "Page size must be greater than zero and less than thirty", 
                "PageSize"));;
    }
}