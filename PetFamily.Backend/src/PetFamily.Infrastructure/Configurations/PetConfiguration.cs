using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities.SpeciesAggregate.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using Constants = PetFamily.Domain.Shared.Constants;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value));

        builder.ComplexProperty(p => p.Name, pb =>
        {
            pb.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LENGTH_NAME)
                .HasColumnName("name");
        });

        builder.OwnsOne(p => p.SpeciesBreed, pb =>
        {
            pb.Property(s => s.SpeciesId)
                .IsRequired()
                .HasConversion(
                    id => id.Value,
                    value => SpeciesId.Create(value))
                .HasColumnName("species_id");
            
            pb.Property(s => s.BreedId)
                .IsRequired()
                .HasConversion(
                    id => id.Value,
                    value => BreedId.Create(value))
                .HasColumnName("breed_id");
        });

        builder.ComplexProperty(p => p.Description, pb =>
        {
            pb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LENGTH_DESCRIPTION)
                .HasColumnName("description");
        });

        builder.Property(p => p.Color)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LENGTH_TITLE)
            .HasColumnName("color");
        
        builder.ComplexProperty(p => p.HealthInformation, pb =>
        {
            pb.Property(h => h.Value)
                .IsRequired()
                .HasMaxLength(HealthInformation.MAX_VALUE_LENGTH)
                .HasColumnName("health_information");
        });

        builder.ComplexProperty(p => p.Address, pb =>
        {
            pb.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(Address.MAX_LENGTH_CITY)
                .HasColumnName("city");
            
            pb.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(Address.MAX_LENGTH_STREET)
                .HasColumnName("street");
            
            pb.Property(a => a.House)
                .IsRequired()
                .HasMaxLength(Address.MAX_LENGTH_HOUSE)
                .HasColumnName("house");
        });
        
        builder.ComplexProperty(p => p.BodySize, pb =>
        {
            pb.Property(a => a.Weight)
                .IsRequired()
                .HasMaxLength(BodySize.MAX_WEIGHT)
                .HasColumnName("city");
            
            pb.Property(a => a.Height)
                .IsRequired()
                .HasMaxLength(BodySize.MAX_HEIGHT)
                .HasColumnName("street");
        });

        builder.ComplexProperty(p => p.PhoneNumber, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(PhoneNumber.MAX_VALUE_LENGTH)
                .HasColumnName("phone_number");
        });

        builder.Property(p => p.IsNeutered)
            .IsRequired()
            .HasColumnName("is_neutered");
        
        builder.Property(p => p.DateOfBirth)
            .IsRequired()
            .HasColumnName("date_of_birth");
        
        builder.Property(p => p.IsVaccinated)
            .IsRequired()
            .HasColumnName("is_vaccinated");
        
        builder.ComplexProperty(p => p.HelpStatus, pb =>
        {
            pb.Property(h => h.Value)
                .IsRequired()
                .HasMaxLength(HelpStatus.MAX_VALUE_LENGTH)
                .HasColumnName("help_status");
        });
        
        builder.OwnsMany(p => p.Requisites, pb =>
        {
            pb.ToJson("requisites");
            
            pb.Property(r => r.AccountNumber)
                .IsRequired(false)
                .HasMaxLength(Requisite.LENGTH_ACCOUNT_NUMBER)
                .HasColumnName("account_number");
                
            pb.Property(r => r.Title)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_LENGTH_TITLE)
                .HasColumnName("title");
                
            pb.Property(r => r.Description)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_LENGTH_DESCRIPTION)
                .HasColumnName("description");
        });
        
        builder.Property(p => p.Created)
            .IsRequired()
            .HasColumnName("created");
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}