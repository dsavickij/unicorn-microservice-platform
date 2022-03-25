using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Polly;
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
    private readonly ILogger<HttpRequestDispatcher> _logger;

    public HttpRequestDispatcher(
        IRestClientProvider restClientProvider,
        IRestRequestProvider restRequestProvider,
        ILogger<HttpRequestDispatcher> logger)
    {
        _clientProvider = restClientProvider;
        _requestProvider = restRequestProvider;
        _logger = logger;
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
        var request = await _requestProvider.GetRestRequestAsync(method, arguments);
        var policy = GetRetryPolicy(method.DeclaringType?.FullName!);

        return await policy.ExecuteAsync(async () =>
        {
            var client = await _clientProvider.GetRestClientAsync(method.DeclaringType!);
            return await client.ExecuteAsync(request);
        });
    }

    private AsyncPolicy GetRetryPolicy(string serviceInterfaceFullName)
    {
        return Policy.Handle<HttpRequestException>().WaitAndRetryAsync(
            new[]
            {
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(20),
                TimeSpan.FromSeconds(30)
            },
            (exception, timespan, context) =>
            {
                _logger.LogWarning($"Failed to call '{serviceInterfaceFullName}'. " +
                    $"Retrying in {timespan.TotalSeconds} seconds");
            });
    }
}
