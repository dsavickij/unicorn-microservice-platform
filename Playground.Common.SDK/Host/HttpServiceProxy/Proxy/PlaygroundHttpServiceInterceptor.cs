using Castle.DynamicProxy;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Playground.Common.SDK.Abstractions.Http.MethodAttributes;
using Playground.Common.SDK.Host.HttpServiceProxy.Rest;
using RestSharp;
using System.Net.Http;
using System.Reflection;

namespace Playground.Common.SDK.Host.HttpServiceProxy.Proxy;

internal class PlaygroundHttpServiceInterceptor : IInterceptor
{
    private readonly ILogger _logger;

    private readonly Type _genericTaskType = typeof(Task<>);
    private readonly Type _asyncReturnType = typeof(Task);
    private readonly Type _syncVoidReturnType = typeof(void);

    private readonly IRestComponentProvider _restProvider;

    public PlaygroundHttpServiceInterceptor(
        IRestComponentProvider restClientProvider,
        ILogger<PlaygroundHttpServiceInterceptor> logger)
    {
        _restProvider = restClientProvider;
        _logger = logger;
    }

    public void Intercept(IInvocation invocation)
    {
        var returnType = invocation.Method.ReturnType;

        try
        {
            if (returnType.BaseType == _asyncReturnType)
            {
                ExecuteAsynchronousInvocation(invocation);
            }
            else
            {
                ExecuteSynchronousInvocation(invocation);
            }

            ////TODO: check for return type: if task, check is it is not faulted, log/throw exception
            //var t = returnType switch
            //{
            //    _ when returnType.BaseType == _asyncReturnType => ExecuteAsynchronousInvocation(invocation),
            //    _ => ExecuteSynchronousInvocation(invocation)
            //};

            //invocation.ReturnValue = t;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private object ExecuteSynchronousInvocation(IInvocation invocation)
    {
        throw new NotImplementedException();
    }

    private void ExecuteAsynchronousInvocation(IInvocation invocation)
    {
        var returnType = invocation.Method.ReturnType;

        var tcsType = typeof(TaskCompletionSource<>).MakeGenericType(returnType.GetGenericArguments()[0]);
        var tcs = Activator.CreateInstance(tcsType);
        invocation.ReturnValue = tcsType.GetProperty("Task")!.GetValue(tcs, null);

        InterceptAsync(invocation).ContinueWith(_ =>
        {
            tcsType.GetMethod("SetResult")!.Invoke(tcs, new object[] { invocation.ReturnValue! });
        });

        //var client = await _restProvider.GetRestClientAsync(invocation.Method.DeclaringType);
        //var request = _restProvider.GetRestRequest(invocation.Method, invocation.Arguments);

        //var response = await client.ExecuteAsync(request);

        //if (response.ResponseStatus is ResponseStatus.Error)
        //{
        //    throw new Exception("Error");
        //}

        //var result = System.Text.Json.JsonSerializer
        //    .Deserialize(response.Content, invocation.Method.ReturnType.GenericTypeArguments.First());

        //await Task.FromResult(result);
    }

    private async Task InterceptAsync(IInvocation invocation)
    {
        var client = await _restProvider.GetRestClientAsync(invocation.Method.DeclaringType);
        var request = _restProvider.GetRestRequest(invocation.Method, invocation.Arguments);

        var response = await client.ExecuteAsync(request);

        if (response.ResponseStatus is ResponseStatus.Error)
        {
            throw new Exception("Error");
        }

        var result = System.Text.Json.JsonSerializer
            .Deserialize(response.Content, invocation.Method.ReturnType.GenericTypeArguments.First());

        invocation.ReturnValue = invocation.Arguments[0] = result;
    }
}