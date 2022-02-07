using System.Reflection;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using RestSharp;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;
using Unicorn.Core.Services.ServiceDiscovery.SDK;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.ServiceDiscovery;

internal interface IServiceDiscoveryClient
{
    Task<HttpServiceConfiguration> GetHttpServiceConfigurationAsync(string serviceHostName);

    Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string serviceHostName);
}

/// <summary>
/// ServiceDiscoveryClient is special class to call ServiceDiscovery to get service configurations.
/// This is done to remove cyclic dependency: if ServiceDiscovery would be called using service's SDK,
/// it would result in attempt to get configuration from ServiceDiscovery, but to do that it would need to
/// get ServiceDiscovery configuration from ServiceDiscovery. The end result is infinite loop. That's why
/// this ServiceDisocveryClient was created. Also, to have strongly-typed configurations, but avoid reference to
/// ServiceDiscovery SDK, the files were added to this project as linked
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

    public async Task<GrpcServiceConfiguration> GetGrpcServiceConfigurationAsync(string serviceHostName)
    {
        _logger?.LogDebug($"Retrieving GRPC service configuration for: {serviceHostName}");

        var request = GetRequest(nameof(IServiceDiscoveryService.GetGrpcServiceConfigurationAsync), serviceHostName);
        var response = await _client.GetAsync<OperationResult<GrpcServiceConfiguration>>(request);

        if (response!.IsSuccess)
        {
            return response.Data!;
        }

        throw new ArgumentException($"Failed to retrieve Grpc service configuration for service {serviceHostName}. " +
            $"Errors: {string.Join("; ", response.Errors.Select(x => x.Message))}");
    }

    public async Task<HttpServiceConfiguration> GetHttpServiceConfigurationAsync(string serviceHostName)
    {
        _logger?.LogDebug($"Retrieving HTTP service configuration for: {serviceHostName}");

        var request = GetRequest(nameof(IServiceDiscoveryService.GetHttpServiceConfigurationAsync), serviceHostName);
        var response = await _client.GetAsync<OperationResult<HttpServiceConfiguration>>(request);

        if (response!.IsSuccess)
        {
            return response.Data!;
        }

        throw new ArgumentException($"Failed to retrieve Http service configuration for service {serviceHostName}. " +
            $"Errors: {string.Join("; ", response.Errors.Select(x => x.Message))}");
    }

    private RestRequest GetRequest(string methodName, string serviceName)
    {
        var method = typeof(IServiceDiscoveryService).GetMethod(methodName)!;
        var path = GetPathTemplate(method);
        var req = new RestRequest(path, Method.Get);

        foreach (var p in method.GetParameters())
        {
            if (path.Contains($"{{{p.Name}}}", StringComparison.OrdinalIgnoreCase))
            {
                req.AddUrlSegment(p.Name!, serviceName);
            }
        }

        return req;
    }

    private string GetPathTemplate(MethodInfo method)
    {
        var attributeType = method.CustomAttributes
            .SingleOrDefault(ca => ca.AttributeType == typeof(UnicornHttpGetAttribute))?.AttributeType;

        return attributeType switch
        {
            null => throw new ArgumentNullException($"Method '{method.Name}' in interface '{nameof(IServiceDiscoveryService)}' " +
                $"is not decorated with '{typeof(UnicornHttpGetAttribute).FullName}' attribute"),
            _ => (method.GetCustomAttribute(attributeType) as UnicornHttpGetAttribute)!.PathTemplate
        };
    }
}
