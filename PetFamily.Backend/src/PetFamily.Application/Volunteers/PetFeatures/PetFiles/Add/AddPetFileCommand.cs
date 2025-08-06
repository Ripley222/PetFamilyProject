namespace PetFamily.Application.Volunteers.PetFeatures.PetFiles.Add;

public record AddPetFileCommand(
    Stream Stream,
    string BucketName,
    string FileName);