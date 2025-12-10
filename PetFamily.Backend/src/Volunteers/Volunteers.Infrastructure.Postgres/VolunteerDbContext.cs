using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Volunteers.Application.Database;
using Volunteers.Domain.VolunteerAggregate.PetEntity;
using Volunteers.Domain.VolunteerAggregate.VolunteerEntity;

namespace Volunteers.Infrastructure.Postgres;

public class VolunteerDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    private const string DATABASE = "Database";
    private const string SCHEMA = "volunteers";
    
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SCHEMA);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerDbContext).Assembly);
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => {builder.AddConsole();});

    public IQueryable<Volunteer> VolunteersRead => Set<Volunteer>().AsNoTracking().AsQueryable();
    public IQueryable<Pet> PetsRead => Set<Pet>().AsNoTracking().AsQueryable();
}