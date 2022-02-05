namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess;

public record GrpcServiceConfigurationEntity
{
    public string Name { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = string.Empty;

    public int Port { get; set; }
}

public record ServiceHostConfigurationEntity
{
    public string Name { get; set; } = string.Empty;

    public IEnumerable<GrpcServiceConfigurationEntity> GrpcConfigurations { get; set; } = Enumerable.Empty<GrpcServiceConfigurationEntity>();

    public int Port { get; set; }
}