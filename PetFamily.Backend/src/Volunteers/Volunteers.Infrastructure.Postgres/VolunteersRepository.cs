using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Volunteers.Application.VolunteersFeatures;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity.ValueObjects;

namespace Volunteers.Infrastructure.Postgres;

public class VolunteersRepository(
    VolunteerDbContext dbContext,
    ILogger<VolunteersRepository> logger) : IVolunteersRepository
{
    public async Task<Result<Guid, Error>> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return volunteer.Id.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Errors.DatabaseErrors.AddError();
        }
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var volunteer = await dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

            if (volunteer is null)
                return Errors.VolunteerErrors.NotFound();

            return volunteer;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Errors.DatabaseErrors.GetError();
        }
    }

    public async Task<Result<Guid, Error>> Save(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.Volunteers.Attach(volunteer);
            await dbContext.SaveChangesAsync(cancellationToken);

            return volunteer.Id.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Errors.DatabaseErrors.SaveError();
        }
    }

    public async Task<Result<Guid, Error>> Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.Volunteers.Remove(volunteer);
            await dbContext.SaveChangesAsync(cancellationToken);

            return volunteer.Id.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Errors.DatabaseErrors.DeleteError();
        }
    }
}