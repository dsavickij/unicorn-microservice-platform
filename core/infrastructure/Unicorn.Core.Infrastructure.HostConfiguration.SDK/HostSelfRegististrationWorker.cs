using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.ServiceDiscovery;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK;

internal class HostSelfRegististrationWorker : IHostedService
{
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

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
