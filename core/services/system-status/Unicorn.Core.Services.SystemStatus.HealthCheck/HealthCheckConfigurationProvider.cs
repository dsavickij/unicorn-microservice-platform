using RestSharp;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Services.SystemStatus.HealthCheck;

public class HealthCheckConfigurationProvider
{
    private const string ConfigurationKey = "ServiceDiscoverySettings:Url";

    private readonly RestClient _client;

    public HealthCheckConfigurationProvider(IConfiguration configuration)
    {
        _client = new RestClient(configuration[ConfigurationKey]);
    }

    public async Task<IEnumerable<HttpServiceConfiguration>> GetAllHttpConfigurationsAsync()
    {
        var req = new RestRequest("api/configurations/http/all", Method.Get);
        var response = await _client.GetAsync<OperationResult<IEnumerable<HttpServiceConfiguration>>>(req);

        if (response!.IsSuccess)
        {
            return response.Data!;
        }

        throw new ArgumentException($"Failed to retrieve Http service configurations. " +
            $"Errors: {string.Join("; ", response.Errors.Select(x => x.Message))}");
    }
}