using Refit;

namespace Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.HttpServices.Proxy.RestComponents;

public interface IRestClientProvider
{
    Task<object> GetRestService(Type httpServiceInterface);
}

internal class RestClientProvider : IRestClientProvider
{
    private readonly IHttpServiceConfigurationProvider _cfgProvider;
    private readonly IHttpClientFactory _httpClientFactory;

    public RestClientProvider(
        IHttpServiceConfigurationProvider httpServiceConfigurationProvider,
        IHttpClientFactory httpClientFactory)
    {
        _cfgProvider = httpServiceConfigurationProvider;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<object> GetRestService(Type httpServiceInterface)
    {
        var cfg = await _cfgProvider.GetHttpServiceConfigurationAsync(httpServiceInterface);
        var httpClient = _httpClientFactory.CreateClient(httpServiceInterface.Name);
        httpClient.BaseAddress = new Uri(cfg.BaseUrl);

        return RestService.For(httpServiceInterface, httpClient);
    }
}
