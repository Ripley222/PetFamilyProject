using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

namespace Volunteers.Application.FileProvider;

public record FileData(
    FilePath FilePath,
    string BucketName);