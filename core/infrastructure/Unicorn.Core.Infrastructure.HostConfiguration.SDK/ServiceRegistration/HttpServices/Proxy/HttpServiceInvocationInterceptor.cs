using System.Reflection;
using System.Text.Json;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Unicorn.Core.Infrastructure.Communication.MessageBroker;
using Unicorn.Core.Infrastructure.Communication.MessageBroker.Attributes;
using Unicorn.Core.Infrastructure.Communication.MessageBroker.Messages;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices.Proxy;

internal class HttpServiceInvocationInterceptor : IInterceptor
{
    private readonly ILogger _logger;
    private readonly Type _taskType = typeof(Task);
    private readonly IRestComponentProvider _restComponentProvider;
    private readonly IOneWayMethodInvocationExecutor _oneWayExecutor;

    public HttpServiceInvocationInterceptor(
        IRestComponentProvider restComponentProvider,
        IOneWayMethodInvocationExecutor oneWayMethodInvocationExecutor,
        ILogger<HttpServiceInvocationInterceptor> logger)
    {
        _restComponentProvider = restComponentProvider;
        _oneWayExecutor = oneWayMethodInvocationExecutor;
        _logger = logger;
    }

    public void Intercept(IInvocation invocation)
    {
        if (invocation.Method.ReturnType.BaseType == _taskType)
        {
            ExecuteGenericTaskReturnTypeInvocation(invocation);
        }
        else
        {
            invocation.ReturnValue = invocation.ReturnValue switch
            {
                _ when invocation.Method.ReturnType == _taskType && IsOneWayMethod(invocation.Method) =>
                    ExecuteTaskReturnTypeOneWayInvocationAsync(invocation),
                _ when invocation.Method.ReturnType == _taskType =>
                    ExecuteTaskReturnTypeInvocationAsync(invocation)
            };
        }
    }

    private async Task ExecuteTaskReturnTypeOneWayInvocationAsync(IInvocation invocation)
    {
        var msg = new UnicornOneWayMessage
        {
            MethodName = invocation.Method.Name,
            Arguments = invocation.Arguments.Select(x => new Argument
            {
                TypeName = x.GetType().AssemblyQualifiedName!,
                Value = x
            })
        };

        var queueName = QueueNameFormatter.GetNamespaceBasedName(invocation.Method);

        await _oneWayExecutor.SendToQueueAsync(queueName, msg);
    }

    private async Task ExecuteTaskReturnTypeInvocationAsync(IInvocation invocation)
    {
        var client = _restComponentProvider.GetRestClientAsync(invocation.Method.DeclaringType!);
        var request = _restComponentProvider.GetRestRequestAsync(invocation.Method, invocation.Arguments);

        await Task.WhenAll(client, request);

        await client.Result.ExecuteAsync(request.Result);
    }

    private void ExecuteGenericTaskReturnTypeInvocation(IInvocation invocation)
    {
        var returnType = invocation.Method.ReturnType;

        var tcsType = typeof(TaskCompletionSource<>).MakeGenericType(returnType.GetGenericArguments()[0]);
        var tcs = Activator.CreateInstance(tcsType);
        invocation.ReturnValue = tcsType.GetProperty("Task")!.GetValue(tcs, null);

        ExecuteGenericTaskReturnTypeInvocationAsync(invocation).ContinueWith(_ =>
        {
            tcsType.GetMethod("SetResult")!.Invoke(tcs, new object[] { invocation.ReturnValue! });
        });
    }

    private bool IsOneWayMethod(MethodInfo method) =>
        method.GetCustomAttributes(true).FirstOrDefault(x => x is UnicornOneWayAttribute) is not null;

    private async Task ExecuteGenericTaskReturnTypeInvocationAsync(IInvocation invocation)
    {
        var client = _restComponentProvider.GetRestClientAsync(invocation.Method.DeclaringType!);
        var request = _restComponentProvider.GetRestRequestAsync(invocation.Method, invocation.Arguments);

        await Task.WhenAll(client, request);

        var response = await client.Result.ExecuteAsync(request.Result);

        // TODO: add response validation, if statusCode 404, 503 etc.

        var result = JsonSerializer.Deserialize(
           response.Content!,
           invocation.Method.ReturnType.GenericTypeArguments.First(),
           new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        invocation.ReturnValue = result;
    }
}
