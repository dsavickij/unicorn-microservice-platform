using MassTransit;
using Unicorn.Core.Infrastructure.Communication.MessageBroker;

namespace Unicorn.Core.Development.ClientHost.Controllers;

public class MyMessageHandler : IUnicornEventHandler<MyMessage>
{
    public Task Consume(ConsumeContext<MyMessage> context)
    {
        var msg = context.Message;

        return context.ConsumeCompleted;
    }
}