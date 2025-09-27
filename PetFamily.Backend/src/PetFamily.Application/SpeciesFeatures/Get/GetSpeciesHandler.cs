using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.SpeciesFeatures.DTOs;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.SpeciesFeatures.Get;

public class GetSpeciesHandler(IReadDbContext readDbContext)
{
    public async Task<Result<GetSpeciesDto, ErrorList>> Handle(CancellationToken cancellationToken)
    {
        var speciesQuery = readDbContext.SpeciesRead;

        var species = await speciesQuery
            .Select(s => new SpeciesDto(
                s.Id.Value,
                s.Name))
            .ToListAsync(cancellationToken);

        return new GetSpeciesDto(species);
    }
}