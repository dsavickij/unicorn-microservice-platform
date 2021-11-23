using Castle.DynamicProxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Playground.ServiceDiscovery.SDK;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Common.SDK.ServiceDiscovery;

public interface IServiceDiscoveryClient : IServiceDiscoveryService
{
}

public class ServiceDiscoveryClient : IServiceDiscoveryClient
{
    private readonly ILogger<ServiceDiscoveryClient> _logger;
    private readonly RestClient _client;

    public ServiceDiscoveryClient(IConfiguration configuration, ILogger<ServiceDiscoveryClient> logger)
    {
        var baseUrl = configuration["ServiceDiscoveryServiceUrl"] ?? throw new ArgumentNullException("ServiceDiscoveryServiceUrl");
        _logger = logger;

        _client = new RestClient(baseUrl);
    }

    public Task<HttpServiceConfiguration> CreateHttpServiceConfiguration(HttpServiceConfiguration httpServiceConfiguration)
    {
        throw new NotImplementedException();
    }

    public Task DeleteHttpServiceConfiguration(string serviceName)
    {
        throw new NotImplementedException();
    }

    public async Task<GrpcServiceConfiguration> GetGrpcServiceConfiguration(string serviceName)
    {
        _logger.LogDebug($"Retrieving GRPC service configuration for: {serviceName}");

        var req = new RestRequest($"GetGrpcServiceConfiguration/{serviceName}", Method.GET);
        return await _client.GetAsync<GrpcServiceConfiguration>(req);
    }

    public async Task<HttpServiceConfiguration> GetHttpServiceConfiguration(string serviceName)
    {
        _logger.LogDebug($"Retrieving GRPC service configuration for: {serviceName}");

        var req = new RestRequest($"GetHttpServiceConfiguration/{serviceName}", Method.GET);
        return await _client.GetAsync<HttpServiceConfiguration>(req);

    }

    public Task<HttpServiceConfiguration> UpdateHttpServiceConfiguration(string serviceName, HttpServiceConfiguration httpServiceConfiguration)
    {
        throw new NotImplementedException();
    }
}
