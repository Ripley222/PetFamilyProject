namespace Volunteers.Application.FileProvider;

public record DeleteFilesData(
    IEnumerable<string> FileNames,
    string BucketName); 