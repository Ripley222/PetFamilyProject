using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteersFeatures;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository(ApplicationDbContext dbContext) : IVolunteersRepository
{
    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default)
    {
        var volunteer = await dbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (volunteer is null)
            return Errors.Volunteer.NotFound();
        
        return volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetByFullName(FullName fullName, CancellationToken cancellationToken = default)
    {
        var volunteer = await dbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.FullName == fullName, cancellationToken);

        if (volunteer is null)
            return Errors.Volunteer.NotFound();
        
        return volunteer;
    }

    public async Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        dbContext.Volunteers.Attach(volunteer);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return volunteer.Id.Value;
    }

    public async Task<Guid> Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        dbContext.Volunteers.Remove(volunteer);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return volunteer.Id.Value;
    }
}