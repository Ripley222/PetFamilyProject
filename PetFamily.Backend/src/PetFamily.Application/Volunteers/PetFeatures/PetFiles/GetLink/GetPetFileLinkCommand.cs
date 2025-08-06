namespace PetFamily.Application.Volunteers.PetFeatures.PetFiles.GetLink;

public record GetPetFileLinkCommand(
    string BucketName,
    string FileName);