using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Entities.VolunteerAggregate.PetEntity.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>, ErrorList>> UploadFiles(IEnumerable<StreamFileData> filesData,
        CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> GetFileLink(FileData fileData, CancellationToken cancellationToken = default);

    Task<UnitResult<ErrorList>> RemoveFile(FileData fileData, CancellationToken cancellationToken = default);
}