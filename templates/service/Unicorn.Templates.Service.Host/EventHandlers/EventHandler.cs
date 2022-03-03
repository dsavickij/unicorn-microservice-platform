using MassTransit;
using Unicorn.Core.Infrastructure.Communication.MessageBroker.Abstractions;

namespace Unicorn.Templates.Service.EventHandlers;

// Change 'object' to the class type this handler will be consuming.
// Subscription to events will be done automatically on service startup

public class ObjectHandler : IUnicornEventHandler<object>
{
    public Task Consume(ConsumeContext<object> context)
    {
        // do your magic

        return context.ConsumeCompleted;
    }
}
