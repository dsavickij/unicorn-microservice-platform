namespace Unicorn.Templates.Service.SDK;

public record Constants
{
    // ServiceHostName must be registered with configuration in ServiceDiscoveryService
    // for inter-service communication over HTTP and gRPC to work
    public const string ServiceHostName = "YOUR_SERVICE_NAME";
}
