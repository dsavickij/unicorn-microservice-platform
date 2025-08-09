using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.ServiceDiscovery.DTOs;
using Unicorn.Core.Infrastructure.Host.SDK.Settings;

namespace Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.ServiceDiscovery;

internal interface IServiceDiscoveryClient
{
    Task<OperationResult<HttpServiceConfiguration>> GetHttpServiceConfigurationAsync(string serviceHostName);

    Task<OperationResult<GrpcServiceConfiguration>> GetGrpcServiceConfigurationAsync(string serviceHostName);

    Task<OperationResult?> CreateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration);

    Task<OperationResult?> CreateGrpcServiceConfigurationAsync(GrpcServiceConfiguration grpcServiceConfiguration);

    Task<OperationResult?> UpdateHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration);

    Task<OperationResult?> UpdateGrpcServiceConfigurationAsync(GrpcServiceConfiguration grpcServiceConfiguration);
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

    public async Task<OperationResult?> CreateGrpcServiceConfigurationAsync(
        GrpcServiceConfiguration grpcServiceConfiguration)
    {
        var req = new RestRequest("api/configurations/grpc", Method.Post);
        req.AddBody(grpcServiceConfiguration);

        return await
            new RestClient(new Uri(InternalBaseHostSettings.ServiceDiscoverySettings.Url))
                .PostAsync<OperationResult>(req);
    }

    public async Task<OperationResult?> CreateHttpServiceConfigurationAsync(
        HttpServiceConfiguration httpServiceConfiguration)
    {
        var req = new RestRequest("api/configurations/http", Method.Post);
        req.AddBody(httpServiceConfiguration);

        var response = await new RestClient(new Uri(InternalBaseHostSettings.ServiceDiscoverySettings.Url))
            .ExecutePostAsync<OperationResult?>(req);

        return response.Data;
    }

    public async Task<OperationResult<GrpcServiceConfiguration>?> GetGrpcServiceConfigurationAsync(
        string serviceHostName)
    {
        _logger?.LogDebug($"Retrieving GRPC service configuration for: {serviceHostName}");

        var request = GetRequest(serviceHostName, "api/configurations/{serviceHostName}/grpc");

        var response = await new RestClient(new Uri(InternalBaseHostSettings.ServiceDiscoverySettings.Url))
            .ExecuteAsync<OperationResult<GrpcServiceConfiguration>?>(request);

        return response.Data;
    }

    // TODO: need to use optional? Configuration may not exist, but we in such case just throw exception
    public async Task<OperationResult<HttpServiceConfiguration>?> GetHttpServiceConfigurationAsync(
        string serviceHostName)
    {
        _logger?.LogDebug($"Retrieving HTTP service configuration for: {serviceHostName}");

        var request = GetRequest(serviceHostName, "api/configurations/{serviceHostName}/http");

        return await new RestClient(new Uri(InternalBaseHostSettings.ServiceDiscoverySettings.Url))
            .GetAsync<OperationResult<HttpServiceConfiguration>>(request);
    }

    public async Task<OperationResult?> UpdateGrpcServiceConfigurationAsync(
        GrpcServiceConfiguration grpcServiceConfiguration)
    {
        var req = new RestRequest("api/configurations/grpc", Method.Put);
        req.AddBody(grpcServiceConfiguration);

        var response = await new RestClient(new Uri(InternalBaseHostSettings.ServiceDiscoverySettings.Url))
            .ExecutePutAsync<OperationResult>(req);

        return response.Data;
    }

    public async Task<OperationResult?> UpdateHttpServiceConfigurationAsync(
        HttpServiceConfiguration httpServiceConfiguration)
    {
        var req = new RestRequest("api/configurations/http", Method.Put);
        req.AddBody(httpServiceConfiguration);

        var response = await new RestClient(new Uri(InternalBaseHostSettings.ServiceDiscoverySettings.Url))
            .ExecutePutAsync<OperationResult>(req);

        return response.Data;
    }

    // TODO: think about adding retry policy if needed
    // private AsyncRetryPolicy GetRetryPolicy()
    // {
    //     return Policy.Handle<HttpRequestException>().WaitAndRetryAsync(
    //         new[] { TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(45) },
    //         (exception, timespan, context) =>
    //         {
    //             _logger.LogWarning(
    //                 $"Failed to call ServiceDiscoveryService. Retrying in {timespan.TotalSeconds} seconds");
    //         });
    // }

    private RestRequest GetRequest(string serviceHostName, string path)
    {
        var req = new RestRequest(path, Method.Get);
        req.AddUrlSegment("serviceHostName", serviceHostName);

        return req;
    }
}