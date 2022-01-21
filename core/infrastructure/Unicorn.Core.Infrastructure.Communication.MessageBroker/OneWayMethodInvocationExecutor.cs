using MassTransit;
using Unicorn.Core.Infrastructure.Communication.MessageBroker.Messages;

namespace Unicorn.Core.Infrastructure.Communication.MessageBroker;

public interface IOneWayMethodInvocationExecutor
{
    Task SendToQueueAsync(string methodFullName, UnicornOneWayMessage message);
}

public class OneWayMethodInvocationExecutor : IOneWayMethodInvocationExecutor
{
    private readonly ISendEndpointProvider _endpointProvider;

    public OneWayMethodInvocationExecutor(ISendEndpointProvider endpointProvider) => _endpointProvider = endpointProvider;

    public async Task SendToQueueAsync(string methodFullName, UnicornOneWayMessage message)
    {
        var endpoint = await _endpointProvider.GetSendEndpoint(new Uri($"queue:{methodFullName}"));
        await endpoint.Send(message);

    }
}
