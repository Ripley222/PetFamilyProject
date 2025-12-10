using Application.Abstraction;
using CSharpFunctionalExtensions;
using Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Species.Database;
using Species.Domain.SpeciesAggregate.ValueObjects;

namespace Species.SpeciesFeatures.Delete;

public sealed class DeleteSpecies
{
    public record DeleteSpeciesByIdCommand(Guid SpeciesId) : ICommand;

    private sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("species/{speciesId:guid}", async (
                [FromRoute] Guid speciesId,
                [FromServices] ICommandHandler<DeleteSpeciesByIdCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteSpeciesByIdCommand(speciesId);
                
                var result = await handler.Handle(command, cancellationToken);

                return Results.Ok(result);
            });
        }
    }

    public sealed class DeleteSpeciesHandler(SpeciesDbContext dbContext) : ICommandHandler<DeleteSpeciesByIdCommand>
    {
        public async Task<UnitResult<ErrorList>> Handle(
            DeleteSpeciesByIdCommand command, CancellationToken cancellationToken = default)
        {
            var id = SpeciesId.Create(command.SpeciesId);

            var species = await dbContext.Species
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            if (species is null)
                return Errors.SpeciesErrors.NotFound().ToErrorList();

            dbContext.Species.Remove(species);
            await dbContext.SaveChangesAsync(cancellationToken);

            return UnitResult.Success<ErrorList>();
        }
    }
}