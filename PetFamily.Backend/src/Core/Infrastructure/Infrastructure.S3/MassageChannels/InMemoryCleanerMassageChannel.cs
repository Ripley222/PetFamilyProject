using System.Threading.Channels;
using Application.Messaging;

namespace Infrastructure.S3.MassageChannels;

public class InMemoryCleanerMassageChannel<TMessage> : IMassageChannel<TMessage>
{
    private readonly Channel<TMessage> _channel = Channel.CreateUnbounded<TMessage>();

    public async Task WriteAsync(TMessage paths, CancellationToken cancellationToken = default)
    {
        await _channel.Writer.WriteAsync(paths, cancellationToken);
    }
    
    public async Task<TMessage> ReadAsync(CancellationToken cancellationToken = default)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }
}