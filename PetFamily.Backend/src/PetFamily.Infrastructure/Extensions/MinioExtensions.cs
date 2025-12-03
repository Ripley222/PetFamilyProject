using CSharpFunctionalExtensions;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Extensions;

public static class MinioExtensions
{
    public static async Task<UnitResult<Error>> CheckObjectAsync(
        this IMinioClient minioClient, GetObjectArgs args, CancellationToken cancellationToken)
    {
        try
        {
            await minioClient.GetObjectAsync(args, cancellationToken);
            
            return UnitResult.Success<Error>();
        }
        catch (MinioException ex)
        {
            return UnitResult.Failure(Error.NotFound("object.not.found", ex.Message));
        }
    }
}