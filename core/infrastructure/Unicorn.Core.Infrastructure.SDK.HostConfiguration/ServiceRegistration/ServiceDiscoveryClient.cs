using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Reflection;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
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
        var baseUrl = Guard.Against.NullOrWhiteSpace(configuration[ServiceDiscoveryConnStringKey], ServiceDiscoveryConnStringKey);
        _client = new RestClient(baseUrl);
        _logger = logger;
    }

    public async Task<GrpcServiceConfiguration> GetGrpcServiceConfiguration(string serviceName)
    {
        _logger.LogDebug($"Retrieving GRPC service configuration for: {serviceName}");

        var request = GetRequest(nameof(IServiceDiscoveryService.GetGrpcServiceConfiguration), serviceName);
        return await _client.GetAsync<GrpcServiceConfiguration>(request);
    }

    public async Task<HttpServiceConfiguration> GetHttpServiceConfiguration(string serviceName)
    {
        _logger.LogDebug($"Retrieving HTTP service configuration for: {serviceName}");

        var request = GetRequest(nameof(IServiceDiscoveryService.GetHttpServiceConfiguration), serviceName);
        return await _client.GetAsync<HttpServiceConfiguration>(request);
    }

    private IRestRequest GetRequest(string methodName, string serviceName)
    {
        var method = typeof(IServiceDiscoveryService).GetMethod(methodName)!;
        var path = GetPathTemplate(method);
        var req = new RestRequest(path, Method.GET, DataFormat.Json);

        foreach (var p in method.GetParameters())
        {
            if (path.Contains($"{{{p.Name}}}"))
                req.AddUrlSegment(p.Name!, serviceName);
        }

        return req;
    }

    private string GetPathTemplate(MethodInfo method)
    {
        var attributeType = method.CustomAttributes
            .SingleOrDefault(ca => ca.AttributeType == typeof(UnicornHttpGet))?.AttributeType;

        if (attributeType is null)
            throw new Exception($"Method '{method.Name}' in interface '{nameof(IServiceDiscoveryService)}' " +
                $"is not decorated with '{typeof(UnicornHttpGet).FullName}' attribute");

        return (method.GetCustomAttribute(attributeType) as UnicornHttpGet)!.PathTemplate;
    }
}