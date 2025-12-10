using CSharpFunctionalExtensions;
using SharedKernel;
using Volunteers.Application.FileProvider;
using Volunteers.Domain.VolunteerAggregate.PetEntity.ValueObjects;

namespace Volunteers.Application.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>, ErrorList>> UploadFiles(IEnumerable<StreamFileData> filesData,
        CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> GetFileLink(FileData fileData, CancellationToken cancellationToken = default);

    Task<UnitResult<ErrorList>> RemoveFile(FileData fileData, CancellationToken cancellationToken = default);

    Task<UnitResult<ErrorList>> RemoveFiles(
        DeleteFilesData deleteFilesData,
        CancellationToken cancellationToken = default);
}