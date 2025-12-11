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

public sealed class GetSpeciesByIds
{
    public record GetSpeciesByIdsQuery(GetSpeciesByIdsDto Dto) : IQuery;
    
    private sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("species/{speciesId:Guid}", async (
                [FromRoute] Guid speciesId,
                [FromServices] GetSpeciesByIdsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetSpeciesByIdsQuery(new GetSpeciesByIdsDto(speciesId));

                var result = await handler.Handle(query, cancellationToken);
                
                return Results.Ok(result);
            });
        }
    }

    public sealed class GetSpeciesByIdsHandler(
        SpeciesDbContext dbContext) : IQueryHandler<SpeciesDto, GetSpeciesByIdsQuery>
    {
        public async Task<Result<SpeciesDto, ErrorList>> Handle(
            GetSpeciesByIdsQuery query, CancellationToken cancellationToken = default)
        {
            var id = SpeciesId.Create(query.Dto.SpeciesId);
            
            var species = await dbContext.Species
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            if (species is null)
                return Errors.SpeciesErrors.NotFound().ToErrorList();
            
            var speciesDto = new SpeciesDto(species.Id.Value, species.Name);

            return speciesDto;
        }
    }
}