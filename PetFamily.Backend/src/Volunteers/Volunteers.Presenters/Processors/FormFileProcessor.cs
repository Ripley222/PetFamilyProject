using Microsoft.AspNetCore.Http;
using Volunteers.Application.VolunteersFeatures.DTOs;

namespace Volunteers.Presenters.Processors;

public class FormFileProcessor : IAsyncDisposable
{
    private readonly List<FileDto> _fileDtos = [];

    public List<FileDto> Process(IFormFile file)
    {
        var stream = file.OpenReadStream();
        var fileDto = new FileDto(stream, file.FileName);

        _fileDtos.Add(fileDto);

        return _fileDtos;
    }

    public List<FileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileDto = new FileDto(stream, file.FileName);

            _fileDtos.Add(fileDto);
        }

        return _fileDtos;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var fileDto in _fileDtos)
        {
            await fileDto.Stream.DisposeAsync();
        }
        
        GC.SuppressFinalize(this);
    }
}