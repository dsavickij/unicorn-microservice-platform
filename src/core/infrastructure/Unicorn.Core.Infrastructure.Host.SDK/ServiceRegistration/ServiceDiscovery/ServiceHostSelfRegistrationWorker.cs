using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.ServiceDiscovery.DTOs;
using Unicorn.Core.Infrastructure.Host.SDK.Settings;

namespace Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.ServiceDiscovery;

internal class ServiceHostSelfRegistrationWorker : IHostedService
{
    private const string UrlConfigurationKey = "ASPNETCORE_URLS";

    private readonly IServiceDiscoveryClient _client;
    private readonly IConfiguration _cfg;
    private readonly ServiceDiscoverySettings _baseHostSettings; // TODO: check it
    private readonly ILogger<ServiceHostSelfRegistrationWorker> _logger;

    public ServiceHostSelfRegistrationWorker(
        IServiceDiscoveryClient client,
        IConfiguration configuration,
        IOptions<ServiceDiscoverySettings> options,
        ILogger<ServiceHostSelfRegistrationWorker> logger)
    {
        _client = client;
        _cfg = configuration;
        _baseHostSettings = options.Value;
        _logger = logger;
    }

    private static bool CanSelfRegistrationProceed =>
        InternalBaseHostSettings.ServiceDiscoverySettings.ExecuteSelfRegistration;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (CanSelfRegistrationProceed)
        {
            var (httpCfg, grpcCfg) = GetServiceHosts();

            await UpsertHttpServiceConfigurationAsync(httpCfg);
            await UpsertGrpcServiceConfigurationAsync(grpcCfg);

            _logger.LogInformation(
                $"ServiceDiscoveryService was successfully called for service host self-registration");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }


    private async Task UpsertHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration)
    {
        var updateResponse = await _client.UpdateHttpServiceConfigurationAsync(httpServiceConfiguration);

        if (updateResponse is { IsSuccess: true })
        {
            return;
        }

        if (updateResponse is { IsSuccess: false, Code: OperationStatusCode.Status400BadRequest })
        {
            var createResponse = await _client.CreateHttpServiceConfigurationAsync(httpServiceConfiguration);

            if (createResponse is { IsSuccess: true })
            {
                return;
            }

            throw new ArgumentException($"Failed to create HTTP service configuration for service " +
                                        $"'{httpServiceConfiguration.ServiceHostName}'. " +
                                        $"Errors: {string.Join("; ", createResponse.Errors.Select(x => x.Message))}");
        }

        throw new ArgumentException($"Failed to update HTTP service configuration for service " +
                                    $"'{httpServiceConfiguration.ServiceHostName}'. " +
                                    $"Errors: {string.Join("; ", updateResponse.Errors.Select(x => x.Message))}");
    }

    private async Task UpsertGrpcServiceConfigurationAsync(GrpcServiceConfiguration grpcServiceConfiguration)
    {
        var updateResponse = await _client.UpdateGrpcServiceConfigurationAsync(grpcServiceConfiguration);

        if (updateResponse is { IsSuccess: true })
        {
            return;
        }

        if (updateResponse is { IsSuccess: false, Code: OperationStatusCode.Status400BadRequest })
        {
            var createResponse = await _client.CreateGrpcServiceConfigurationAsync(grpcServiceConfiguration);

            if (createResponse is { IsSuccess: true })
            {
                return;
            }

            throw new ArgumentException($"Failed to create gRPC service configuration for service " +
                                        $"'{grpcServiceConfiguration.ServiceHostName}'. " +
                                        $"Errors: {string.Join("; ", createResponse.Errors.Select(x => x.Message))}");
        }

        throw new ArgumentException($"Failed to create gRPC service configuration for service " +
                                    $"'{grpcServiceConfiguration.ServiceHostName}'. " +
                                    $"Errors: {string.Join("; ", updateResponse.Errors.Select(x => x.Message))}");
    }

    private (HttpServiceConfiguration http, GrpcServiceConfiguration grpc) GetServiceHosts()
    {
        var (httpUri, httpsUri) = GetHttpUris();

        var httpCfg = new HttpServiceConfiguration
        {
            ServiceHostName = InternalBaseHostSettings.ServiceHostName, BaseUrl = httpUri.AbsoluteUri
        };

        var grpcCfg = new GrpcServiceConfiguration
        {
            ServiceHostName = InternalBaseHostSettings.ServiceHostName, BaseUrl = httpsUri.AbsoluteUri
        };

        return (httpCfg, grpcCfg);
    }

    private (Uri httpUri, Uri httpsUri) GetHttpUris()
    {
        const string urlConfigurationKey = "ASPNETCORE_URLS";
        const int maxMinUrlCount = 2;

        var urls = _cfg[urlConfigurationKey]?.Split(";") ?? throw new Exception(); // TODO: fix exception throwing

        if (urls.Length is > maxMinUrlCount or < maxMinUrlCount)
        {
            throw new ArgumentException(
                $"Defined number of URLs for configuration key '{urlConfigurationKey}' is " +
                $"'{urls.Length}' when it must be equal to '{maxMinUrlCount}'");
        }

        return (GetHttpUri(urls), GetHttpsUri(urls));
    }

    private Uri GetHttpsUri(string[] urls)
    {
        var httpsUrls = urls.Where(url => url.StartsWith("https://", StringComparison.OrdinalIgnoreCase));

        return httpsUrls.Count() is not 1
            ? throw new ArgumentException(
                $"Only one HTTPS URL can be defined in value for configuration key '{UrlConfigurationKey}'")
            : new Uri(httpsUrls.Single());
    }

    private Uri GetHttpUri(string[] urls)
    {
        var httpUrls = urls.Where(url => url.StartsWith("http://", StringComparison.OrdinalIgnoreCase));

        return httpUrls.Count() is not 1
            ? throw new ArgumentException(
                $"Only one HTTP URL can be defined in value for configuration key '{UrlConfigurationKey}'")
            : new Uri(httpUrls.Single());
    }
}