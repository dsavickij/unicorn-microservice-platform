namespace Unicorn.Core.Infrastructure.Security.IAM.Settings;

public class AuthenticationSettings
{
    public string AuthorityUrl { get; set; } = string.Empty;
    public ClientCredentials ClientCredentials { get; set; } = new();
}

public class ClientCredentials
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Scopes { get; set; } = string.Empty;
}
