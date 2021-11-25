using RestSharp;
using System.Reflection;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.HttpServices.Rest.Components;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.HttpServices.Rest;

internal interface IRestComponentProvider : IRestClientProvider, IRestRequestProvider
{
}

internal class RestComponentProvider : IRestComponentProvider
{
    private readonly IRestClientProvider _clientProvider;
    private readonly IRestRequestProvider _requestProvider;

    public RestComponentProvider(
        IRestClientProvider httpServiceConfigurationProvider,
        IRestRequestProvider restRequestComponentProvider)
    {
        _clientProvider = httpServiceConfigurationProvider;
        _requestProvider = restRequestComponentProvider;
    }

    public async Task<IRestClient> GetRestClientAsync(Type httpServiceInterface)
    {
        return await _clientProvider.GetRestClientAsync(httpServiceInterface);
    }

    public IRestRequest GetRestRequest(MethodInfo httpServiceMethod, IList<object> methodArguments)
    {
        return _requestProvider.GetRestRequest(httpServiceMethod, methodArguments);
    }
}
