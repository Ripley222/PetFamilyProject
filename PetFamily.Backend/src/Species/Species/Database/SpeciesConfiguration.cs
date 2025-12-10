using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel;
using Species.Domain.SpeciesAggregate.ValueObjects;

namespace Species.Database;

public class SpeciesConfiguration : IEntityTypeConfiguration<Domain.SpeciesAggregate.Species>
{
    public void Configure(EntityTypeBuilder<Domain.SpeciesAggregate.Species> builder)
    {
        builder.ToTable("species");
        
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value));

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LENGTH_NAME)
            .HasColumnName("name");
        
        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("species_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}