using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.SpeciesFeatures.DTOs;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.SpeciesFeatures.BreedsFeatures.Get;

public class GetBreedsBySpeciesIdHandler(IReadDbContext readDbContext)
{
    public async Task<Result<GetBreedsDto, ErrorList>> Handle(
        GetBreedsByIdSpeciesQuery query,
        CancellationToken cancellationToken)
    {
        var speciesQuery = readDbContext.SpeciesRead;

        speciesQuery = speciesQuery.Where(s => s.Id == SpeciesId.Create(query.SpeciesId));

        var breeds = await speciesQuery
            .Select(s => s.Breeds
                .Select(b => new BreedDto(
                    b.Id.Value,
                    b.Name)))
            .ToListAsync(cancellationToken);

        return new GetBreedsDto(breeds[0]);
    }
}