namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.ServiceDiscovery.DTOs;

/// <summary>
/// The class is a copy of Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations.GrpcServiceConfiguration
/// These files needs to be kept in sync to ensure service configuration retrieval from ServiceDiscovery service
/// </summary>
internal record GrpcServiceConfiguration
{
    public string ServiceHostName { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = string.Empty;

    public int Port { get; set; }
}
