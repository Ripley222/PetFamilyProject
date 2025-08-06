using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger) : IFileProvider
{
    public async Task<Result<string, ErrorList>> UploadFile(
        StreamFileData streamFileData, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(streamFileData.FileData.BucketName);
            
            var bucketExist =  await minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);
            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(streamFileData.FileData.BucketName);
                
                await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
            
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(streamFileData.FileData.BucketName)
                .WithStreamData(streamFileData.Stream)
                .WithObjectSize(streamFileData.Stream.Length)
                .WithObject(streamFileData.FileData.FileName);
            
            var result  = await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            
            return result.ObjectName;
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
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithObject(fileData.FileName);
            
            var objectStat = await minioClient.StatObjectAsync(statObjectArgs, cancellationToken);
            if (objectStat.Size == 0)
                return Error.Failure("file.get", "File not exist").ToErrorList(); 
            
            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithObject(fileData.FileName)
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

    public async Task<Result<string, ErrorList>> RemoveFile(
        FileData fileData, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithObject(fileData.FileName);
            
            await minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            
            return fileData.FileName;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error delete minio file");
            return Error.Failure("file.delete", "Error delete minio file").ToErrorList();
        }
    }
}