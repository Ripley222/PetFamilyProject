using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity.ValueObjects;
using PetFamily.Domain.Entities.VolunteerAggregate.VolunteerEntity;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value));

        builder.ComplexProperty(v => v.FullName, vb =>
        {
            vb.Property(f => f.FirstName)
                .IsRequired()
                .HasMaxLength(FullName.MAX_FIRSTNAME_LENGTH)
                .HasColumnName("first_name");
            
            vb.Property(f => f.MiddleName)
                .IsRequired()
                .HasMaxLength(FullName.MAX_MIDDLENAME_LENGTH)
                .HasColumnName("middle_name");
            
            vb.Property(f => f.LastName)
                .IsRequired()
                .HasMaxLength(FullName.MAX_LASTNAME_LENGTH)
                .HasColumnName("last_name");
        });

        builder.ComplexProperty(v => v.EmailAddress, vb =>
        {
            vb.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(EmailAddress.MAX_LENGTH_EMAIL)
                .HasColumnName("email_address");
        });

        builder.ComplexProperty(v => v.Description, vb =>
        {
            vb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LENGTH_DESCRIPTION)
                .HasColumnName("description");
        });

        builder.Property(v => v.YearsOfExperience)
            .IsRequired()
            .HasColumnName("years_of_experience");

        builder.ComplexProperty(v => v.PhoneNumber, vb =>
        {
            vb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(PhoneNumber.MAX_VALUE_LENGTH)
                .HasColumnName("phone_number");
        });

        builder.OwnsMany(v => v.Socials, vb =>
        {
            vb.ToJson("socials");
            
            vb.Property(sn => sn.Title)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_LENGTH_TITLE)
                .HasColumnName("title");

            vb.Property(sn => sn.Link)
                .IsRequired(false)
                .HasMaxLength(SocialNetwork.MAX_LENGTH_LINK)
                .HasColumnName("link");
        });

        builder.OwnsMany(v => v.Requisites, vb =>
        {
            vb.ToJson("requisites");
            
            vb.Property(r => r.AccountNumber)
                .IsRequired(false)
                .HasMaxLength(Requisite.LENGTH_ACCOUNT_NUMBER)
                .HasColumnName("account_number");
                
            vb.Property(r => r.Title)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_LENGTH_TITLE)
                .HasColumnName("title");
                
            vb.Property(r => r.Description)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_LENGTH_DESCRIPTION)
                .HasColumnName("description");
        });
        
        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}