using Application.Options;

namespace Infrastructure.S3.Options;

public class MinioOptions
{
    public const string MINIO = "Minio";

    public string Endpoint { get; init; } = string.Empty;
    public string AccessKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public bool WithSsl { get; init; }

    public MinioBucketOptions BucketOptions { get; init; } = null!;
}

public class MinioBucketOptions : IMinioBucketOptions
{
    public string BucketPhotos { get; init; } = string.Empty;
}