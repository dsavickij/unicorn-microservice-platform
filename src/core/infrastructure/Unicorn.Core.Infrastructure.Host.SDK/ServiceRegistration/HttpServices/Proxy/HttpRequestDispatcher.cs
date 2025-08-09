using System.Reflection;
using Microsoft.Extensions.Logging;
using Refit;
using Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.HttpServices.Proxy.RestComponents;

namespace Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.HttpServices.Proxy;

internal interface IHttpRequestDispatcher
{
    Task<object> SendHttpRequestAsync(MethodInfo method, object[] arguments);
    Task SendNoResultHttpRequestAsync(MethodInfo method, object[] arguments);
}

internal class HttpRequestDispatcher : IHttpRequestDispatcher
{
    private readonly IRestClientProvider _clientProvider;
    private readonly IHttpServiceConfigurationProvider _httpServiceConfigurationProvider;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HttpRequestDispatcher> _logger;

    public HttpRequestDispatcher(
        IHttpClientFactory httpClientFactory,
        IRestClientProvider restClientProvider,
        IHttpServiceConfigurationProvider httpServiceConfigurationProvider,
        ILogger<HttpRequestDispatcher> logger)
    {
        _clientProvider = restClientProvider;
        _httpServiceConfigurationProvider = httpServiceConfigurationProvider;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<object> SendHttpRequestAsync(MethodInfo method, object[] arguments)
    {
        var reqBuilder = RequestBuilder.ForType(method.DeclaringType!);

        var func = reqBuilder.BuildRestResultFuncForMethod(method.Name, method.GetParameters()
            .Select(x => x.ParameterType)
            .ToArray());

        var cfg = await _httpServiceConfigurationProvider.GetHttpServiceConfigurationAsync(method.DeclaringType!);

        var task = (Task)func(RestService.CreateHttpClient(cfg.BaseUrl, null), arguments);

        await task;

        return task.GetType().GetProperty("Result")?.GetValue(task) ?? throw new Exception("");
    }

    public async Task SendNoResultHttpRequestAsync(MethodInfo method, object[] arguments)
    {
        var client = await _clientProvider.GetRestService(method.DeclaringType!);

        var task = (Task)method.Invoke(client, arguments) ?? throw new Exception("error");

        await task;
    }
}
