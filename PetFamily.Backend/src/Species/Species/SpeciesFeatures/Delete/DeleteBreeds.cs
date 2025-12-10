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

public sealed class DeleteBreeds
{
    public record DeleteBreedsByIdCommand(Guid SpeciesId, Guid BreedId) : ICommand;

    private sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("species/{speciesId:guid}/breeds/{breedId:Guid}", async (
                [FromRoute] Guid speciesId,
                [FromRoute] Guid breedId,
                [FromServices] DeleteBreedsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteBreedsByIdCommand(speciesId, breedId);
                
                var result = await handler.Handle(command, cancellationToken);

                return Results.Ok(result);
            });
        }
    }

    public sealed class DeleteBreedsHandler(SpeciesDbContext dbContext) : ICommandHandler<DeleteBreedsByIdCommand>
    {
        public async Task<UnitResult<ErrorList>> Handle(
            DeleteBreedsByIdCommand command, CancellationToken cancellationToken = default)
        {
            var speciesId = SpeciesId.Create(command.SpeciesId);
            var breedId = BreedId.Create(command.BreedId);

            var species = await dbContext.Species
                .Include(s => s.Breeds.Where(b => b.Id == breedId))
                .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);

            if (species is null)
                return Errors.SpeciesErrors.NotFound().ToErrorList();
            
            var breed = species.Breeds.FirstOrDefault(b => b.Id == breedId);
            if (breed is null)
                return Errors.BreedsErrors.NotFound().ToErrorList();

            species.RemoveBreed(breed);

            await dbContext.SaveChangesAsync(cancellationToken);

            return UnitResult.Success<ErrorList>();
        }
    }
}