using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

public record CatalogHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
}