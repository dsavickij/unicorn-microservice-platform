using MassTransit;

namespace Unicorn.Core.Infrastructure.Communication.SDK.OneWay;

public interface IUnicornEventPublisher
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
}

public class UnicornEventPublisher : IUnicornEventPublisher
{
    private readonly IPublishEndpoint _publisher;

    public UnicornEventPublisher(IPublishEndpoint publisher) => _publisher = publisher;

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        await _publisher.Publish(message, cancellationToken); 
    }
}
