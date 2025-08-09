namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess;

internal static class SeedData
{
    public static class ServiceHosts
    {
        public static ServiceHostEntity ServiceDiscovery => new() { Name = "Unicorn.Core.Services.ServiceDiscovery" };
        public static ServiceHostEntity ServiceHost => new() { Name = "Unicorn.Core.Development.Service" };
        public static ServiceHostEntity Discount => new() { Name = "Unicorn.eShop.Discount" };
        public static ServiceHostEntity Cart => new() { Name = "Unicorn.eShop.Cart" };
    }

    public static class HttpServiceConfigurations
    {
        public static HttpServiceConfigurationEntity ServiceDiscovery => new()
        {
            ServiceHostName = ServiceHosts.ServiceDiscovery.Name,
            BaseUrl = "http://localhost:5081",
            Id = new Guid("dfbb13c5-beb5-4e0a-81dd-0416021e74a5"),
        };

        public static HttpServiceConfigurationEntity ServiceHost => new()
        {
            ServiceHostName = ServiceHosts.ServiceHost.Name,
            BaseUrl = "https://localhost:7004",
            Id = new Guid("d1ff1c8e-6da2-4fba-ba77-eafe550e69ff"),
        };
    }

    public static class GrpcServiceConfigurations
    {
        public static GrpcServiceConfigurationEntity ServiceHost => new()
        {
            ServiceHostName = ServiceHosts.ServiceHost.Name,
            BaseUrl = "https://localhost:7287",
            Id = new Guid("315baac5-d4b1-4bc4-bc88-fca6773da448"),
        };

        public static GrpcServiceConfigurationEntity Discount => new()
        {
            ServiceHostName = ServiceHosts.Discount.Name,
            BaseUrl = "https://unicorn.eshop.discount:443",
            Id = new Guid("1efa697f-349e-4cc1-a083-a3ab28c08418"),
        };
    }
}
