using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, ErrorList>> UploadFile(StreamFileData streamFileData, CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> GetFileLink(FileData fileData, CancellationToken cancellationToken = default);

    Task<Result<string, ErrorList>> RemoveFile(FileData fileData, CancellationToken cancellationToken = default);
}