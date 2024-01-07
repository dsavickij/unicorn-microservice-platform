using System.Security.Claims;

namespace Unicorn.Core.Infrastructure.Security.IAM.AuthenticationContext;

public interface IUnicornIdentity
{
    public string AccessToken { get; }
    public Guid UserId { get; }
    public string FirstName { get; }
    public string LastName { get; }
}

internal class UnicornIdentity : IUnicornIdentity
{
    private const string FirstNameClaimType = "firstName";
    private const string LastNameClaimType = "lastName";
    private const string UserIdClaimType = "sub";

    private readonly IEnumerable<Claim> _claims;

    public UnicornIdentity(string accessToken, IEnumerable<Claim> claims)
    {
        AccessToken = accessToken;
        _claims = claims;
    }

    public string AccessToken { get; }

    public Guid UserId => Guid.TryParse(_claims.First(x => x.Type == UserIdClaimType).Value, out var guid) ? guid : Guid.Empty;

    public string FirstName => _claims.First(x => x.Type == FirstNameClaimType).Value ?? string.Empty;

    public string LastName => _claims.First(x => x.Type == LastNameClaimType).Value ?? string.Empty;
}
