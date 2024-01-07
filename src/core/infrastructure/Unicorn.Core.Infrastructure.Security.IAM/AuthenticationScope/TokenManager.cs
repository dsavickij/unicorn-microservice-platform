using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Unicorn.Core.Infrastructure.Security.IAM.Settings;

namespace Unicorn.Core.Infrastructure.Security.IAM.AuthenticationScope;

internal interface ITokenManager
{
    Task<string> GetTokenAsync();
}

internal class TokenManager : ITokenManager
{
    private readonly AuthenticationSettings _settings;

    public TokenManager(IOptions<AuthenticationSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<string> GetTokenAsync()
    {
        var client = new HttpClient()
        {
            BaseAddress = new Uri(_settings.AuthorityUrl),
        };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/connect/token", UriKind.Relative),
        };

        var authenticationString = $"{_settings.ClientCredentials.ClientId}:{_settings.ClientCredentials.ClientSecret}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));

        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

        var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") });
        request.Content = content;

        // TODO check response and do something if it was not successful
        var response = await client.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<Token>(json)?.AccessToken ?? throw new ArgumentException("");
    }
}

internal record Token
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
}
