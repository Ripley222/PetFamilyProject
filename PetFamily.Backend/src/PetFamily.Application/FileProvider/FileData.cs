using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;

namespace PetFamily.Application.FileProvider;

public record FileData(
    FilePath FilePath,
    string BucketName);