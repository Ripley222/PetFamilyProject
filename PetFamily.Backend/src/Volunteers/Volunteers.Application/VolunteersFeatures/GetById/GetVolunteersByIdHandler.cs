using Application.Abstraction;
using Application.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Volunteers.Application.Database;
using Volunteers.Application.VolunteersFeatures.DTOs;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Application.VolunteersFeatures.GetById;

public class GetVolunteersByIdHandler(
    IReadDbContext readDbContext,
    IValidator<GetVolunteersByIdQuery> validator) : IQueryHandler<VolunteerDto, GetVolunteersByIdQuery>
{
    public async Task<Result<VolunteerDto, ErrorList>> Handle(
        GetVolunteersByIdQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid is false)
            return validationResult.GetErrors();

        var volunteersQuery = readDbContext.VolunteersRead;
        
        volunteersQuery = volunteersQuery
            .Where(v => v.Id == VolunteerId.Create(query.VolunteerId));

        var volunteer = await volunteersQuery
            .Select(v => new VolunteerDto(
                v.Id.Value,
                v.FullName.FirstName,
                v.FullName.MiddleName,
                v.FullName.LastName,
                v.Description.Value,
                v.YearsOfExperience,
                v.PhoneNumber.Value,
                v.Requisites.Select(r => 
                    new RequisitesDto
                    {
                        AccountNumber = r.AccountNumber,
                        Description = r.Description,
                        Title = r.Title
                    }),
                v.Socials.Select(s => new SocialNetworksDto
                {
                    Link = s.Link,
                    Title = s.Title
                })))
            .FirstOrDefaultAsync(cancellationToken);
        
        if (volunteer is null)
            return Errors.VolunteerErrors.NotFound().ToErrorList();

        return volunteer;
    }
}