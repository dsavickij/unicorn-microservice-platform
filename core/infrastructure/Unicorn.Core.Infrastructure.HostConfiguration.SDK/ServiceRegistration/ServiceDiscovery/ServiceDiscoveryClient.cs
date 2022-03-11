using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using RestSharp;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.ServiceDiscovery.DTOs;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.ServiceDiscovery;

internal interface IServiceDiscoveryClient
{
    Task<HttpServiceConfiguration> GetHttpServiceConfigurationAsync(string serviceHostName);

    Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string serviceHostName);

    Task CreateHttpServiceConfiguration(HttpServiceConfiguration configuration);

    Task CreateGrpcServiceConfiguration(GrpcServiceConfiguration configuration);
}

/// <summary>
/// ServiceDiscoveryClient is special class to call ServiceDiscovery to get service configurations.
/// This is done to remove cyclic dependency: if ServiceDiscovery would be called using service's SDK,
/// it would result in attempt to get configuration from ServiceDiscovery, but to do that it would need to
/// get ServiceDiscovery configuration from ServiceDiscovery. The end result is infinite loop. That's why
/// this ServiceDisocveryClient was created. Also, to have strongly-typed configurations, but avoid reference to
/// ServiceDiscovery SDK, copies of classes were made
/// </summary>
internal class ServiceDiscoveryClient : IServiceDiscoveryClient
{
    private readonly ILogger<ServiceDiscoveryClient> _logger;
    private readonly RestClient _client;

    public ServiceDiscoveryClient(string serviceDiscoveryUrl, ILogger<ServiceDiscoveryClient> logger)
    {
        var baseUrl = Guard.Against.NullOrWhiteSpace(serviceDiscoveryUrl, serviceDiscoveryUrl);
        _client = new RestClient(new Uri(baseUrl));
        _logger = logger;
    }

    public async Task CreateGrpcServiceConfiguration(GrpcServiceConfiguration configuration)
    {
        var req = new RestRequest("api/configurations/grpc", Method.Post);
        req.AddBody(configuration);

        var response = await _client.PostAsync<OperationResult>(req);

        if (response?.IsSuccess is not true)
        {
            // do something
        }
    }

    public async Task CreateHttpServiceConfiguration(HttpServiceConfiguration configuration)
    {
        var req = new RestRequest("api/configurations/http", Method.Post);
        req.AddBody(configuration);

        var response = await _client.PostAsync<OperationResult>(req);

        if (response?.IsSuccess is not true)
        {
            // do something
        }
    }

    public async Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string serviceHostName)
    {
        _logger?.LogDebug($"Retrieving GRPC service configuration for: {serviceHostName}");

        var request = GetRequest(serviceHostName, "api/configurations/{serviceHostName}/grpcServiceConfiguration");

        var response = await _client.GetAsync<OperationResult<GrpcServiceConfiguration>>(request);

        if (response!.IsSuccess)
        {
            return response.Data!;
        }

        throw new ArgumentException($"Failed to retrieve Grpc service configuration for service '{serviceHostName}'. " +
            $"Errors: {string.Join("; ", response.Errors.Select(x => x.Message))}");
    }

    public async Task<HttpServiceConfiguration> GetHttpServiceConfigurationAsync(string serviceHostName)
    {
        _logger?.LogDebug($"Retrieving HTTP service configuration for: {serviceHostName}");

        var request = GetRequest(serviceHostName, "api/configurations/{serviceHostName}/httpServiceConfiguration");
        var response = await _client.GetAsync<OperationResult<HttpServiceConfiguration>>(request);

        if (response!.IsSuccess)
        {
            return response.Data!;
        }

        throw new ArgumentException($"Failed to retrieve Http service configuration for service '{serviceHostName}'. " +
            $"Errors: {string.Join("; ", response.Errors.Select(x => x.Message))}");
    }

    private RestRequest GetRequest(string serviceHostName, string path)
    {
        var req = new RestRequest(path, Method.Get);
        req.AddUrlSegment("serviceHostName", serviceHostName);

        return req;
    }
}
