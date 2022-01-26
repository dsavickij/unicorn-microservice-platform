using MassTransit;
using Unicorn.Core.Infrastructure.Communication.MessageBroker.Messages;

namespace Unicorn.Core.Infrastructure.Communication.MessageBroker;

internal interface IQueueMessageDispatcher
{
    Task SendAsync(string methodFullName, UnicornQueueMessage message);
}

internal class QueueMessageDispatcher : IQueueMessageDispatcher
{
    private readonly ISendEndpointProvider _endpointProvider;

    public QueueMessageDispatcher(ISendEndpointProvider endpointProvider) => _endpointProvider = endpointProvider;

    public async Task SendAsync(string methodFullName, UnicornQueueMessage message)
    {
        var endpoint = await _endpointProvider.GetSendEndpoint(new Uri($"queue:{methodFullName}"));
        await endpoint.Send(message);
    }
}
