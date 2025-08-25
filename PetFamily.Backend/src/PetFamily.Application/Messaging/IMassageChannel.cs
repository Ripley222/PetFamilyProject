using PetFamily.Application.FileProvider;

namespace PetFamily.Application.Messaging;

public interface IMassageChannel<TMessage>
{
    Task WriteAsync(TMessage paths, CancellationToken cancellationToken = default);

    Task<TMessage> ReadAsync(CancellationToken cancellationToken = default);
}