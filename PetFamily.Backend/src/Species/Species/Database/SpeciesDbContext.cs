using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Species.Database;

public class SpeciesDbContext(IConfiguration configuration) : DbContext
{
    private const string DATABASE = "Database";
    private const string SCHEMA = "species";
    
    public DbSet<Domain.SpeciesAggregate.Species> Species { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(DATABASE));
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SCHEMA);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SpeciesDbContext).Assembly);
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => {builder.AddConsole();});
}