using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.Communication.SDK.OneWay.Queue.Message;

namespace Unicorn.Core.Infrastructure.Communication.SDK.OneWay.Queue;

internal class QueueMessageHandler : IConsumer<UnicornQueueMessage>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IControllerMethodProvider _methodProvider;

    public QueueMessageHandler(IServiceProvider serviceProvider, IControllerMethodProvider methodProvider)
    {
        _serviceProvider = serviceProvider;
        _methodProvider = methodProvider;
    }

    public async Task Consume(ConsumeContext<UnicornQueueMessage> context)
    {
        var method = _methodProvider.GetOneWayMethod(context.Message.MethodName, context.Message.Arguments.Count());
        var controller = _serviceProvider.GetRequiredService(method.DeclaringType!);

        var arguments = context.Message.Arguments
            .Select(x => Convert.ChangeType(x.Value, Type.GetType(x.TypeName)!))
            .ToArray();

        await (method.Invoke(controller, arguments) as Task)!;

        await context.ConsumeCompleted;
    }
}