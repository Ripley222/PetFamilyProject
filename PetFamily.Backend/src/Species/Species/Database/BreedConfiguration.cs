using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel;
using Species.Domain.SpeciesAggregate;
using Species.Domain.SpeciesAggregate.ValueObjects;

namespace Species.Database;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");
        
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Create(value));

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LENGTH_NAME)
            .HasColumnName("name");
    }
}