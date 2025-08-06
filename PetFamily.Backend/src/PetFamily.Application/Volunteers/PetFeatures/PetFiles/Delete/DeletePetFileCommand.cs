namespace PetFamily.Application.Volunteers.PetFeatures.PetFiles.Delete;

public record DeletePetFileCommand(
    string BucketName,
    string FileName);