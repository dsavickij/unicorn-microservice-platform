namespace Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

public record GrpcServiceConfiguration
{
    public string Name { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = string.Empty;

    public int Port { get; set; }
}
