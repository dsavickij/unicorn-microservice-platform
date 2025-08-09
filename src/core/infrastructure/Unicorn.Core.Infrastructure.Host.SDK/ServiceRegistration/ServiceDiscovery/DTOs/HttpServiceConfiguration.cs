namespace Unicorn.Core.Infrastructure.Host.SDK.ServiceRegistration.ServiceDiscovery.DTOs;

/// <summary>
/// The class is a copy of Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations.HttpServiceConfiguration
/// These files needs to be kept in sync to ensure service configuration retrieval from ServiceDiscovery service
/// </summary>
internal record HttpServiceConfiguration
{
    public string ServiceHostName { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}
