using Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings;

public record DiscountHostSettings : BaseHostSettings
{
    public string DbConnectionString { get; set; } = string.Empty;
}