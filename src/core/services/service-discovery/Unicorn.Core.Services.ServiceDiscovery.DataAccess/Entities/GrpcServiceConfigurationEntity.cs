namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess;

public record GrpcServiceConfigurationEntity
{
    public Guid Id { get; set; }
    public string BaseUrl { get; set; } = string.Empty;
    public int Port { get; set; }

    public ServiceHostEntity? ServiceHost { get; set; }
    public string ServiceHostName { get; set; } = string.Empty;
}
