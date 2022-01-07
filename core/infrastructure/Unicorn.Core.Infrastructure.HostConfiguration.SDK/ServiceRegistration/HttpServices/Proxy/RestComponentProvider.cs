using System.Reflection;
using RestSharp;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices.Proxy.RestComponents;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.HttpServices.Proxy;

internal interface IRestComponentProvider : IRestClientProvider, IRestRequestProvider
{
}

internal class RestComponentProvider : IRestComponentProvider
{
    private readonly IRestClientProvider _clientProvider;
    private readonly IRestRequestProvider _requestProvider;

    public RestComponentProvider(IRestClientProvider restClientProvider, IRestRequestProvider restRequestProvider)
    {
        _clientProvider = restClientProvider;
        _requestProvider = restRequestProvider;
    }

    public async Task<RestClient> GetRestClientAsync(Type httpServiceInterface) =>
        await _clientProvider.GetRestClientAsync(httpServiceInterface);

    public async Task<RestRequest> GetRestRequestAsync(MethodInfo httpServiceMethod, IList<object> methodArguments) =>
       await _requestProvider.GetRestRequestAsync(httpServiceMethod, methodArguments);
}
