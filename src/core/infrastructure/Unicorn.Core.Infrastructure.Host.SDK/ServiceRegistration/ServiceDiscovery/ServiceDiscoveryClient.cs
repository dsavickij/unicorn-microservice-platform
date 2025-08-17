using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneOf;
using OneOf.Types;
using Polly;
using Polly.Retry;
using RestSharp;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.ServiceDiscovery.DTOs;
using Unicorn.Core.Infrastructure.Host.SDK.Settings;

namespace Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.ServiceDiscovery;

internal interface IServiceDiscoveryClient
{
    Task<OneOf<OperationResult<HttpServiceConfiguration>, None>> GetHttpServiceConfigurationAsync(
        string serviceHostName);

    Task<OneOf<OperationResult<GrpcServiceConfiguration>, None>> GetGrpcServiceConfigurationAsync(
        string serviceHostName);

    Task<OneOf<OperationResult, None>> CreateHttpServiceConfigurationAsync(
        HttpServiceConfiguration httpServiceConfiguration);

    Task<OneOf<OperationResult, None>> CreateGrpcServiceConfigurationAsync(
        GrpcServiceConfiguration grpcServiceConfiguration);

    Task<OneOf<OperationResult, None>> UpdateHttpServiceConfigurationAsync(
        HttpServiceConfiguration httpServiceConfiguration);

    Task<OneOf<OperationResult, None>> UpdateGrpcServiceConfigurationAsync(
        GrpcServiceConfiguration grpcServiceConfiguration);
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
    private readonly ServiceDiscoverySettings _settings;

    public ServiceDiscoveryClient(IOptions<ServiceDiscoverySettings> baseSettings,
        ILogger<ServiceDiscoveryClient> logger)
    {
        _settings = baseSettings.Value;
        _logger = logger;
    }

    public async Task<OneOf<OperationResult, None>> CreateGrpcServiceConfigurationAsync(
        GrpcServiceConfiguration grpcServiceConfiguration)
    {
        var request = new RestRequest("api/configurations/grpc", Method.Post);
        request.AddBody(grpcServiceConfiguration);
        var client = new RestClient(new Uri(InternalBaseHostSettings.ServiceDiscoverySettings.Url));

        var response = await GetRetryPolicy().ExecuteAsync(() => client.ExecutePostAsync<OperationResult>(request));

        return response.Data is null ? default(None) : response.Data;
    }

    public async Task<OneOf<OperationResult, None>> CreateHttpServiceConfigurationAsync(
        HttpServiceConfiguration httpServiceConfiguration)
    {
        var request = new RestRequest("api/configurations/http", Method.Post);
        request.AddBody(httpServiceConfiguration);
        var client = new RestClient(new Uri(InternalBaseHostSettings.ServiceDiscoverySettings.Url));

        var response = await GetRetryPolicy().ExecuteAsync(() => client.ExecutePostAsync<OperationResult?>(request));

        return response.Data is null ? default(None) : response.Data;
    }

    public async Task<OneOf<OperationResult<GrpcServiceConfiguration>, None>> GetGrpcServiceConfigurationAsync(
        string serviceHostName)
    {
        _logger?.LogDebug($"Retrieving GRPC service configuration for: {serviceHostName}");

        var request = GetRequest(serviceHostName, "api/configurations/{serviceHostName}/grpc");
        var client = new RestClient(new Uri(InternalBaseHostSettings.ServiceDiscoverySettings.Url));

        var response = await GetRetryPolicy()
            .ExecuteAsync(() => client.ExecuteAsync<OperationResult<GrpcServiceConfiguration>?>(request));

        return response.Data is null ? default(None) : response.Data;
    }

    public async Task<OneOf<OperationResult<HttpServiceConfiguration>, None>> GetHttpServiceConfigurationAsync(
        string serviceHostName)
    {
        _logger?.LogDebug($"Retrieving HTTP service configuration for: {serviceHostName}");

        var request = GetRequest(serviceHostName, "api/configurations/{serviceHostName}/http");
        var client = new RestClient(new Uri(InternalBaseHostSettings.ServiceDiscoverySettings.Url));

        var response = await GetRetryPolicy()
            .ExecuteAsync(() => client.ExecuteGetAsync<OperationResult<HttpServiceConfiguration>>(request));

        return response.Data is null ? default(None) : response.Data;
    }

    public async Task<OneOf<OperationResult, None>> UpdateGrpcServiceConfigurationAsync(
        GrpcServiceConfiguration grpcServiceConfiguration)
    {
        var req = new RestRequest("api/configurations/grpc", Method.Put);
        req.AddBody(grpcServiceConfiguration);

        var client = new RestClient(new Uri(InternalBaseHostSettings.ServiceDiscoverySettings.Url));
        var response = await GetRetryPolicy().ExecuteAsync(() => client.ExecutePutAsync<OperationResult>(req));

        return response.Data is null ? default(None) : response.Data;
    }

    public async Task<OneOf<OperationResult, None>> UpdateHttpServiceConfigurationAsync(
        HttpServiceConfiguration httpServiceConfiguration)
    {
        var req = new RestRequest("api/configurations/http", Method.Put);
        req.AddBody(httpServiceConfiguration);

        var client = new RestClient(new Uri(InternalBaseHostSettings.ServiceDiscoverySettings.Url));
        var response = await GetRetryPolicy().ExecuteAsync(() => client.ExecutePutAsync<OperationResult>(req));

        return response.Data is null ? default(None) : response.Data;
    }

    private AsyncRetryPolicy GetRetryPolicy()
    {
        return Policy.Handle<HttpRequestException>().WaitAndRetryAsync(
            [TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(45)],
            (exception, timespan, context) =>
            {
                _logger.LogWarning(
                    "Failed to call ServiceDiscoveryService. Retrying in {TotalSeconds} seconds",
                    timespan.TotalSeconds);
            });
    }

    private RestRequest GetRequest(string serviceHostName, string path)
    {
        var req = new RestRequest(path, Method.Get);
        req.AddUrlSegment("serviceHostName", serviceHostName);

        return req;
    }
}