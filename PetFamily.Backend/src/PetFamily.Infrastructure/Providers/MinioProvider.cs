using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger) : IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLELISM = 10;

    public async Task<Result<IReadOnlyList<FilePath>, ErrorList>> UploadFiles(
        IEnumerable<StreamFileData> filesData,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var filesList = filesData.ToList();

        try
        {
            await IfBucketsNotExistCreateBucket(filesList, cancellationToken);

            var tasks = filesList.Select(async file =>
                await PutObject(file, semaphoreSlim, cancellationToken));

            var pathResult = await Task.WhenAll(tasks);
            if (pathResult.Any(p => p.IsFailure))
                return pathResult.First().Error;

            var results = pathResult.Select(p => p.Value).ToList();
            return results;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading minio file");
            return Error.Failure("file.upload", "Error uploading minio file").ToErrorList();
        }
    }

    public async Task<Result<string, ErrorList>> GetFileLink(
        FileData fileData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithObject(fileData.FilePath.Value)
                .WithExpiry(60 * 60 * 24);

            var url = await minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);

            return url;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting minio file link");
            return Error.Failure("file.get", "Error getting minio file file").ToErrorList();
        }
    }

    public async Task<UnitResult<ErrorList>> RemoveFile(
        FileData fileData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithObject(fileData.FilePath.Value)
                .WithCallbackStream(_ => {});

            var objectStat = await minioClient.CheckObjectAsync(getObjectArgs, cancellationToken);
            if (objectStat.IsFailure)
                return objectStat.Error.ToErrorList();

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithObject(fileData.FilePath.Value);

            await minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            
            return Result.Success<ErrorList>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error delete minio file");
            return Error.Failure("file.delete", "Error delete minio file").ToErrorList();
        }
    }

    public async Task<UnitResult<ErrorList>> RemoveFiles(
        DeleteFilesData deleteFilesData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectsArgs = new RemoveObjectsArgs()
                .WithBucket(deleteFilesData.BucketName)
                .WithObjects(deleteFilesData.FileNames.ToList());

            await minioClient.RemoveObjectsAsync(removeObjectsArgs, cancellationToken);

            return Result.Success<ErrorList>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error delete minio files");
            return Error.Failure("files.delete", "Error delete minio files").ToErrorList();
        }
    }

    private async Task<Result<FilePath, ErrorList>> PutObject(
        StreamFileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken = default)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.FileData.BucketName)
            .WithStreamData(fileData.Stream)
            .WithObjectSize(fileData.Stream.Length)
            .WithObject(fileData.FileData.FilePath.Value);

        try
        {
            await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return fileData.FileData.FilePath;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Fail to upload file in minio with path {path} in buket {bucket}",
                fileData.FileData.FilePath.Value,
                fileData.FileData.BucketName);

            return Error.Failure("file.upload", "Fail to upload file to minio").ToErrorList();
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task IfBucketsNotExistCreateBucket(
        IEnumerable<StreamFileData> filesData,
        CancellationToken cancellationToken = default)
    {
        HashSet<string> bucketsName = [..filesData.Select(file => file.FileData.BucketName)];

        foreach (var bucketName in bucketsName)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);
            if (bucketExist is false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
        }
    }
}