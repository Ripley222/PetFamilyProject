using Application.Abstraction;
using CSharpFunctionalExtensions;
using Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Species.Contracts.DTOs;
using Species.Database;
using Species.Domain.SpeciesAggregate.ValueObjects;

namespace Species.SpeciesFeatures.Get;

public sealed class GetSpeciesBreedsByIds
{
    public record GetSpeciesBreedsByIdsQuery(GetSpeciesBreedByIdsDto Dto) : IQuery;
    
    private sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("species/{speciesId:Guid}/breeds/{breedId:Guid}", async (
                [FromRoute] Guid speciesId,
                [FromRoute] Guid breedId,
                [FromServices] GetSpeciesBreedsByIdsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetSpeciesBreedsByIdsQuery(new GetSpeciesBreedByIdsDto(speciesId, breedId));

                var result = await handler.Handle(query, cancellationToken);

                return Results.Ok(result);
            });
        }
    }
    
    public sealed class GetSpeciesBreedsByIdsHandler(
        SpeciesDbContext dbContext) : IQueryHandler<SpeciesBreedDto, GetSpeciesBreedsByIdsQuery>
    {
        public async Task<Result<SpeciesBreedDto, ErrorList>> Handle(
            GetSpeciesBreedsByIdsQuery query, CancellationToken cancellationToken = default)
        {
            var speciesId = SpeciesId.Create(query.Dto.SpeciesId);
            var breedId = BreedId.Create(query.Dto.BreedId);

            var species = await dbContext.Species
                .Include(s => s.Breeds.Where(b => b.Id == breedId))
                .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);

            if (species is null)
                return Errors.SpeciesErrors.NotFound().ToErrorList();

            var breed = species.Breeds.FirstOrDefault(b => b.Id == breedId);

            if (breed is null)
                return Errors.BreedsErrors.NotFound().ToErrorList();

            var speciesBreedDto = new SpeciesBreedDto(
                species.Id.Value,
                breed.Id.Value,
                species.Name,
                breed.Name);

            return speciesBreedDto;
        }
    }
}