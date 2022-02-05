namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess;

public record HttpServiceConfigurationEntity
{
    public string Name { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}