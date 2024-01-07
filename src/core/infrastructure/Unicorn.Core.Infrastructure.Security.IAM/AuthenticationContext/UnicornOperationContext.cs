namespace Unicorn.Core.Infrastructure.Security.IAM.AuthenticationContext;

public static class UnicornOperationContext
{
    private static readonly AsyncLocal<IUnicornIdentity> _identity = new();

    public static void Set(IUnicornIdentity identity) => _identity.Value = identity;

    public static Guid UserId => _identity.Value?.UserId ?? Guid.Empty;

    public static string FirstName => _identity.Value?.FirstName ?? string.Empty;

    public static string LastName => _identity.Value?.LastName ?? string.Empty;

    public static string AccessToken => _identity.Value?.AccessToken ?? string.Empty;

    internal static IUnicornIdentity Identity => _identity.Value!;
}
