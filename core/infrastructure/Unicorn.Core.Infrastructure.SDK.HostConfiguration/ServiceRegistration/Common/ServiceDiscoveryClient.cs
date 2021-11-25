using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.Common;

internal interface IServiceDiscoveryClient
{
    Task<HttpServiceConfiguration> GetHttpServiceConfiguration(string serviceName);
    Task<GrpcServiceConfiguration> GetGrpcServiceConfiguration(string serviceName);
}

internal class ServiceDiscoveryClient : IServiceDiscoveryClient
{
    private const string ServiceDiscoveryConnStringKey = "ServiceDiscoveryServiceUrl";

    private readonly ILogger<ServiceDiscoveryClient> _logger;
    private readonly RestClient _client;

    public ServiceDiscoveryClient(IConfiguration configuration, ILogger<ServiceDiscoveryClient> logger)
    {
        var baseUrl = configuration[ServiceDiscoveryConnStringKey] ?? throw new ArgumentNullException(ServiceDiscoveryConnStringKey);
        _logger = logger;

        _client = new RestClient(baseUrl);
    }

    public async Task<GrpcServiceConfiguration> GetGrpcServiceConfiguration(string serviceName)
    {
        _logger.LogDebug($"Retrieving GRPC service configuration for: {serviceName}");

        var req = new RestRequest($"GetGrpcServiceConfiguration/{serviceName}", Method.GET, DataFormat.Json);
        return await _client.GetAsync<GrpcServiceConfiguration>(req);
    }

    public async Task<HttpServiceConfiguration> GetHttpServiceConfiguration(string serviceName)
    {
        _logger.LogDebug($"Retrieving HTTP service configuration for: {serviceName}");

        var req = new RestRequest($"GetHttpServiceConfiguration/{serviceName}", Method.GET, DataFormat.Json);
        return await _client.GetAsync<HttpServiceConfiguration>(req);
    }
}