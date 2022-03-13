using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.ServiceDiscovery;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK;

internal class HostSelfRegististrationWorker : IHostedService
{
    private const string UrlConfigurationKey = "ASPNETCORE_URLS";

    private readonly IServiceDiscoveryClient _client;
    private readonly IConfiguration _cfg;
    private readonly ILogger<HostSelfRegististrationWorker> _logger;

    public HostSelfRegististrationWorker(
        IServiceDiscoveryClient client,
        IConfiguration configuration,
        ILogger<HostSelfRegististrationWorker> logger)
    {
        _client = client;
        _cfg = configuration;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var urls = _cfg["ASPNETCORE_URLS"];

        _logger.LogDebug(urls);

        var (httpUri, httpsUri) = GetServiceHostUrls();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private (Uri httpUri, Uri httpsUri) GetServiceHostUrls()
    {
        const string urlConfigurationKey = "ASPNETCORE_URLS";
        const int maxMinUrlCount = 2;

        var urls = _cfg[urlConfigurationKey]?.Split(";") ?? throw new Exception();

        if (urls.Length is > maxMinUrlCount or < maxMinUrlCount)
        {
            throw new ArgumentException($"Defined number of URLs for configuration key '{urlConfigurationKey}' is " +
                $"'{urls.Length}' when it must be equal to '{maxMinUrlCount}'");
        }

        return (GetHttpUri(urls), GetHttpsUri(urls));
    }

    private Uri GetHttpsUri(string[] urls)
    {
        var httpsUrls = urls.Where(url => url.StartsWith("https", StringComparison.OrdinalIgnoreCase));

        return httpsUrls.Count() is not 1
            ? throw new ArgumentException($"Only one HTTPS URL can be defined in value for configuration key '{UrlConfigurationKey}'")
            : new Uri(httpsUrls.Single());
    }

    private Uri GetHttpUri(string[] urls)
    {
        var httpUrls = urls.Where(url => url.StartsWith("http", StringComparison.OrdinalIgnoreCase));

        return httpUrls.Count() is not 1
            ? throw new ArgumentException($"Only one HTTP URL can be defined in value for configuration key '{UrlConfigurationKey}'")
            : new Uri(httpUrls.Single());
    }
}
