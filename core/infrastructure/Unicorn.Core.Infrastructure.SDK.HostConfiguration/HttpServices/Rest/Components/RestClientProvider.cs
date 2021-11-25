using RestSharp;
using RestSharp.Serializers.SystemTextJson;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.HttpServices.Rest.Components;

public interface IRestClientProvider
{
    Task<IRestClient> GetRestClientAsync(Type httpServiceInterface);
}

internal class RestClientProvider : IRestClientProvider
{
    private readonly IHttpServiceConfigurationProvider _cfgProvider;

    public RestClientProvider(IHttpServiceConfigurationProvider httpServiceConfigurationProvider)
    {
        _cfgProvider = httpServiceConfigurationProvider;
    }

    public async Task<IRestClient> GetRestClientAsync(Type httpServiceInterface)
    {
        var cfg = await _cfgProvider.GetHttpServiceConfiguration(httpServiceInterface);
        return new RestClient(cfg.BaseUrl).UseSystemTextJson();
    }
}
