using System.Reflection;
using System.Text.Json;
using RestSharp;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices.Proxy.RestComponents;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices.Proxy;

internal interface IHttpRequestDispatcher
{
    Task<object> SendHttpRequestAsync(MethodInfo method, object[] arguments);
    Task SendNoResultHttpRequestAsync(MethodInfo method, object[] arguments);
}

internal class HttpRequestDispatcher : IHttpRequestDispatcher
{
    private readonly IRestClientProvider _clientProvider;
    private readonly IRestRequestProvider _requestProvider;

    public HttpRequestDispatcher(IRestClientProvider restClientProvider, IRestRequestProvider restRequestProvider)
    {
        _clientProvider = restClientProvider;
        _requestProvider = restRequestProvider;
    }

    public async Task<object> SendHttpRequestAsync(MethodInfo method, object[] arguments)
    {
        var response = await SendAsync(method, arguments);

        // TODO: add response validation, if statusCode 404, 503 etc.

        return JsonSerializer.Deserialize(
            response.Content!,
            method.ReturnType.GenericTypeArguments.First(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task SendNoResultHttpRequestAsync(MethodInfo method, object[] arguments) =>
         await SendHttpRequestAsync(method, arguments);

    private async Task<RestResponse> SendAsync(MethodInfo method, object[] arguments)
    {
        var client = _clientProvider.GetRestClientAsync(method.DeclaringType!);
        var request = _requestProvider.GetRestRequestAsync(method, arguments);

        await Task.WhenAll(client, request);

        return await client.Result.ExecuteAsync(request.Result);
    }
}
