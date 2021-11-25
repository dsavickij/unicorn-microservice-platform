using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Playground.Core.Infrastructure.SDK.HostConfiguration.HttpServices.Rest;

internal class HttpServiceInterceptor : IInterceptor
{
    private readonly ILogger _logger;
    private readonly Type _taskType = typeof(Task);
    private readonly IRestComponentProvider _restComponentProvider;

    public HttpServiceInterceptor(
        IRestComponentProvider restComponentProvider,
        ILogger<HttpServiceInterceptor> logger)
    {
        _restComponentProvider = restComponentProvider;
        _logger = logger;
    }

    public void Intercept(IInvocation invocation)
    {
        var returnType = invocation.Method.ReturnType;

        //TODO: check for return type: if task, check if it is not faulted, log/throw exception
        if (returnType.BaseType == _taskType)
        {
            ExecuteGenericTaskReturnTypeInvocation(invocation);
        }
        else if (returnType == _taskType)
        {
            invocation.ReturnValue = ExecuteTaskReturnTypeInvocation(invocation);
        }
    }

    private async Task ExecuteTaskReturnTypeInvocation(IInvocation invocation)
    {
        var client = await _restComponentProvider.GetRestClientAsync(invocation.Method.DeclaringType!);
        var request = _restComponentProvider.GetRestRequest(invocation.Method, invocation.Arguments);

        await client.ExecuteAsync(request);
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

    private async Task ExecuteGenericTaskReturnTypeInvocationAsync(IInvocation invocation)
    {
        var client = await _restComponentProvider.GetRestClientAsync(invocation.Method.DeclaringType!);
        var request = _restComponentProvider.GetRestRequest(invocation.Method, invocation.Arguments);

        var response = await client.ExecuteAsync(request);

        var result = JsonSerializer.Deserialize(
            response.Content,
            invocation.Method.ReturnType.GenericTypeArguments.First(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        invocation.ReturnValue = invocation.Arguments[0] = result;
    }
}