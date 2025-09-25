using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.GetWithPagination;

public class GetVolunteersHandler(
    IReadDbContext readDbContext,
    IValidator<GetVolunteersQuery> validator)
{
    public async Task<Result<GetVolunteersDto?, ErrorList>> Handle(
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
                v.Requisites,
                v.Socials))
            .ToListAsync(cancellationToken);

        return new GetVolunteersDto(volunteers);
    }
}