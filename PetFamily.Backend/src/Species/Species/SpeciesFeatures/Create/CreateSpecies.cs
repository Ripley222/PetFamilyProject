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

namespace Species.SpeciesFeatures.Create;

public sealed class CreateSpecies
{
    public record CreateSpeciesCommand(string SpeciesName) : ICommand;

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("species", async (
                [FromBody] string speciesName,
                [FromServices] CreateSpeciesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateSpeciesCommand(speciesName);

                var result = await handler.Handle(command, cancellationToken);

                return Results.Ok(result.Value);
            });
        }
    }

    public sealed class CreateSpeciesHandler(SpeciesDbContext dbContext) : ICommandHandler<Guid, CreateSpeciesCommand>
    {
        public async Task<Result<Guid, ErrorList>> Handle(
            CreateSpeciesCommand command, CancellationToken cancellationToken = default)
        {
            var speciesId = SpeciesId.New();
            var speciesName = command.SpeciesName;

            var species = Domain.SpeciesAggregate.Species.Create(speciesId, speciesName);
            if (species.IsFailure)
                return species.Error.ToErrorList();
            
            var speciesExists = await dbContext.Species
                .FirstOrDefaultAsync(s => s.Name == speciesName, cancellationToken);

            if (speciesExists is not null)
                return Errors.SpeciesErrors.AlreadyExist().ToErrorList();

            dbContext.Species.Add(species.Value);
            await dbContext.SaveChangesAsync(cancellationToken);

            return species.Value.Id.Value;
        }
    }
}