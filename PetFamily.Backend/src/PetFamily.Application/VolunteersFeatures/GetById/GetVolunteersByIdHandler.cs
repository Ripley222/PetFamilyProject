using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.VolunteersFeatures.DTOs;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersFeatures.GetById;

public class GetVolunteersByIdHandler(
    IReadDbContext readDbContext,
    IValidator<GetVolunteersByIdQuery> getVolunteersByIdCommandValidator)
{
    public async Task<Result<VolunteerDto?, ErrorList>> Handle(
        GetVolunteersByIdQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await getVolunteersByIdCommandValidator.ValidateAsync(query, cancellationToken);
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
                v.Requisites,
                v.Socials))
            .FirstOrDefaultAsync(cancellationToken);

        return volunteer;
    }
}