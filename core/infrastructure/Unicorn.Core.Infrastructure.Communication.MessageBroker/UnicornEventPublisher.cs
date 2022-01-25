using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Core.Infrastructure.Communication.MessageBroker;

public interface IUnicornEventPublisher
{
    Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class;
}

public class UnicornEventPublisher : IUnicornEventPublisher
{
    private readonly IPublishEndpoint _publisher;

    public UnicornEventPublisher(IPublishEndpoint publisher) => _publisher = publisher;

    public async Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        await _publisher.Publish(message, cancellationToken); 
    }
}
