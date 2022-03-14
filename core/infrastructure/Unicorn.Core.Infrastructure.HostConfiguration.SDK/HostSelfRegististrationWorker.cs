﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.ServiceDiscovery;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.ServiceDiscovery.DTOs;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK;

internal class HostSelfRegististrationWorker : IHostedService
{
    private const string UrlConfigurationKey = "ASPNETCORE_URLS";
    private const int TimeToWaitForServiceDiscoveryInMillis = 5000;

    private readonly IServiceDiscoveryClient _client;
    private readonly IConfiguration _cfg;
    private readonly BaseHostSettings _baseHostSettings;
    private readonly ILogger<HostSelfRegististrationWorker> _logger;

    public HostSelfRegististrationWorker(
        IServiceDiscoveryClient client,
        IConfiguration configuration,
        ILogger<HostSelfRegististrationWorker> logger)
    {
        _client = client;
        _cfg = configuration;
        _baseHostSettings = configuration
            .GetRequiredSection(configuration["HostSettingsConfigurationSectionName"])
            .Get<BaseHostSettings>();
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (CanSelfRegistrationProceed())
        {
            var (httpCfg, grpcCfg) = GetServiceHostConfigurations();

            // give time for ServiceDiscovery to be ready
            await Task.Delay(TimeToWaitForServiceDiscoveryInMillis, cancellationToken);

            var httpCfgResult = UpsertHttpServiceConfigurationAsync(httpCfg);
            var grpcCfgResult = UpsertGrpcServiceConfigurationAsync(grpcCfg);

            await Task.WhenAll(httpCfgResult, grpcCfgResult);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private bool CanSelfRegistrationProceed() => _baseHostSettings.ServiceDiscoverySettings.ExecuteSelfRegistration;

    private async Task UpsertHttpServiceConfigurationAsync(HttpServiceConfiguration httpServiceConfiguration)
    {
        var updateResponse = await _client.UpdateHttpServiceConfigurationAsync(httpServiceConfiguration);

        if (updateResponse is { IsSuccess: false })
        {
            var createResponse = await _client.CreateHttpServiceConfigurationAsync(httpServiceConfiguration);

            if (createResponse is { IsSuccess: false })
            {
                throw new ArgumentException($"Failed to upsert HTTP service configuration for service " +
                    $"'{httpServiceConfiguration.ServiceHostName}'. " +
                    $"Errors: {string.Join("; ", createResponse.Errors.Select(x => x.Message))}");
            }
        }
    }

    private async Task UpsertGrpcServiceConfigurationAsync(GrpcServiceConfiguration grpcServiceConfiguration)
    {
        var updateResponse = await _client.UpdateGrpcServiceConfigurationAsync(grpcServiceConfiguration);

        if (updateResponse is { IsSuccess: false })
        {
            var createResponse = await _client.CreateGrpcServiceConfigurationAsync(grpcServiceConfiguration);

            if (createResponse is { IsSuccess: false })
            {
                throw new ArgumentException($"Failed to upsert gRPC service configuration for service " +
                    $"'{grpcServiceConfiguration.ServiceHostName}'. " +
                    $"Errors: {string.Join("; ", createResponse.Errors.Select(x => x.Message))}");
            }
        }
    }

    private (HttpServiceConfiguration http, GrpcServiceConfiguration grpc) GetServiceHostConfigurations()
    {
        var (httpUri, httpsUri) = GetHttpUris();

        var httpCfg = new HttpServiceConfiguration
        {
            ServiceHostName = _baseHostSettings.ServiceHostName,
            BaseUrl = httpUri.AbsoluteUri
        };

        var grpcCfg = new GrpcServiceConfiguration
        {
            ServiceHostName = _baseHostSettings.ServiceHostName,
            BaseUrl = httpsUri.AbsoluteUri
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
            throw new ArgumentException($"Defined number of URLs for configuration key '{urlConfigurationKey}' is " +
                $"'{urls.Length}' when it must be equal to '{maxMinUrlCount}'");
        }

        return (GetHttpUri(urls), GetHttpsUri(urls));
    }

    private Uri GetHttpsUri(string[] urls)
    {
        var httpsUrls = urls.Where(url => url.StartsWith("https://", StringComparison.OrdinalIgnoreCase));

        return httpsUrls.Count() is not 1
            ? throw new ArgumentException($"Only one HTTPS URL can be defined in value for configuration key '{UrlConfigurationKey}'")
            : new Uri(httpsUrls.Single());
    }

    private Uri GetHttpUri(string[] urls)
    {
        var httpUrls = urls.Where(url => url.StartsWith("http://", StringComparison.OrdinalIgnoreCase));

        return httpUrls.Count() is not 1
            ? throw new ArgumentException($"Only one HTTP URL can be defined in value for configuration key '{UrlConfigurationKey}'")
            : new Uri(httpUrls.Single());
    }
}
