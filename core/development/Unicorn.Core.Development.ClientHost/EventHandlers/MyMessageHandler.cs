using MassTransit;
using Unicorn.Core.Infrastructure.Communication.MessageBroker.Abstractions;

namespace Unicorn.Core.Development.ClientHost.EventHandlers;

public class MyMessageHandler : IUnicornEventHandler<MyMessage>
{
    public Task Consume(ConsumeContext<MyMessage> context)
    {
        var msg = context.Message;

        return context.ConsumeCompleted;
    }
}