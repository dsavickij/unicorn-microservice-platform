namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess;

public record HttpServiceConfigurationEntity
{
    public Guid Id { get; set; }
    public string BaseUrl { get; set; } = string.Empty;

    public ServiceHostEntity? ServiceHost{ get; set; }
    public string ServiceHostName { get; set; } = string.Empty;
}