using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.Communication.MessageBroker.Messages;

namespace Unicorn.Core.Infrastructure.Communication.MessageBroker;

public class UnicornOneWayMessageConsumer : IConsumer<UnicornOneWayMessage>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOneWayControllerMethodProvider _controllerMethodProvider;

    public UnicornOneWayMessageConsumer(IServiceProvider serviceProvider, IOneWayControllerMethodProvider controllerMethodProvider)
    {
        _serviceProvider = serviceProvider;
        _controllerMethodProvider = controllerMethodProvider;
    }

    public async Task Consume(ConsumeContext<UnicornOneWayMessage> context)
    {
        var method = _controllerMethodProvider.GetOneWayMethod(context.Message.MethodName, context.Message.Arguments.Count());

        var controlller = _serviceProvider.GetRequiredService(method.DeclaringType!);

        var arguments = context.Message.Arguments
            .Select(x => Convert.ChangeType(x.Value, Type.GetType(x.TypeName)!))
            .ToArray();

        await (method.Invoke(controlller, arguments) as Task)!;

        await context.ConsumeCompleted;
    }
}
