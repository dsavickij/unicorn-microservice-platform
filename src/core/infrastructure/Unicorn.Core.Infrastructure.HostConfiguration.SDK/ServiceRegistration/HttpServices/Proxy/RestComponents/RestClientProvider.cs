using RestSharp;
using RestSharp.Authenticators.OAuth2;
using Unicorn.Core.Infrastructure.Security.IAM.AuthenticationContext;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices.Proxy.RestComponents;

public interface IRestClientProvider
{
    Task<RestClient> GetRestClientAsync(Type httpServiceInterface);
}

internal class RestClientProvider : IRestClientProvider
{
    private readonly IHttpServiceConfigurationProvider _cfgProvider;

    public RestClientProvider(IHttpServiceConfigurationProvider httpServiceConfigurationProvider)
    {
        _cfgProvider = httpServiceConfigurationProvider;
    }

    public async Task<RestClient> GetRestClientAsync(Type httpServiceInterface)
    {
        var cfg = await _cfgProvider.GetHttpServiceConfigurationAsync(httpServiceInterface);

        var options = new RestClientOptions
        {
            BaseUrl = new Uri(cfg.BaseUrl),
            ThrowOnAnyError = true,
        };

        return new RestClient(options)
        {
            Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                UnicornOperationContext.AccessToken, "Bearer")
        };
    }
}
