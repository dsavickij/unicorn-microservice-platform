using RestSharp;
using RestSharp.Serializers.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Common.SDK.Host.HttpServiceProxy.Rest.Components;

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
