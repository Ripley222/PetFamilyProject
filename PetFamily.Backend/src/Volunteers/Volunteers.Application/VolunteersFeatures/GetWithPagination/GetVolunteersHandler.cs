using Application.Abstraction;
using Application.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Volunteers.Application.Database;
using Volunteers.Application.VolunteersFeatures.DTOs;

namespace Volunteers.Application.VolunteersFeatures.GetWithPagination;

public class GetVolunteersHandler(
    IReadDbContext readDbContext,
    IValidator<GetVolunteersQuery> validator) : IQueryHandler<GetVolunteersDto, GetVolunteersQuery>
{
    public async Task<Result<GetVolunteersDto, ErrorList>> Handle(
        GetVolunteersQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid is false)
            return validationResult.GetErrors();

        var volunteersQuery = readDbContext.VolunteersRead;

        volunteersQuery = volunteersQuery
            .Take(query.PageSize)
            .Skip((query.Page - 1) * query.PageSize);

        var volunteers = await volunteersQuery
            .Select(v => new VolunteerDto(
                v.Id.Value,
                v.FullName.FirstName,
                v.FullName.MiddleName,
                v.FullName.LastName,
                v.Description.Value,
                v.YearsOfExperience,
                v.PhoneNumber.Value,
                v.Requisites.Select(r => new RequisitesDto
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
            .ToListAsync(cancellationToken);
        
        if (volunteers.Count == 0)
            return Errors.VolunteerErrors.NotFound().ToErrorList();

        return new GetVolunteersDto(volunteers);
    }
}