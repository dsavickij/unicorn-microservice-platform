using System.IdentityModel.Tokens.Jwt;
using Unicorn.Core.Infrastructure.Security.IAM.AuthenticationContext;

namespace Unicorn.Core.Infrastructure.Security.IAM.AuthenticationScope;

public interface IAuthenticationScope : IDisposable
{
    IAuthenticationScope EnterServiceUserScope();
}

internal class UnicornAuthenticationScope : IAuthenticationScope
{
    private readonly ITokenManager _tokenManager;
    private IUnicornIdentity _current;

    public UnicornAuthenticationScope(ITokenManager tokenManager)
    {
        _tokenManager = tokenManager;
    }

    public IAuthenticationScope EnterServiceUserScope()
    {
        _current = UnicornOperationContext.Identity;

        var token = AsyncHelpers.RunSync(() => _tokenManager.GetTokenAsync());
        var jwt = new JwtSecurityToken(token);
        var scopeIdentity = new UnicornIdentity(token, jwt.Claims);

        UnicornOperationContext.Set(scopeIdentity);

        return this;
    }

    public void Dispose()
    {
        UnicornOperationContext.Set(_current);
    }
}