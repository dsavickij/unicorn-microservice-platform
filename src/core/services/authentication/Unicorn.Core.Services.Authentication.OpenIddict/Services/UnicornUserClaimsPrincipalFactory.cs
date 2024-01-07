using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using Unicorn.Core.Services.Authentication.OpenIddict.Models;

namespace Unicorn.Core.Services.Authentication.OpenIddict.Services;

internal static class UnicornClaimTypes
{
    public const string FirstName = "firstName";
    public const string LastName = "lastName";
}

public class UnicornUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
{
    public UnicornUserClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
    {
    }

    public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        var principal = await base.CreateAsync(user);
        var identity = (ClaimsIdentity)principal.Identity;

        identity.AddClaim(new Claim(UnicornClaimTypes.FirstName, user.FirstName));
        identity.AddClaim(new Claim(UnicornClaimTypes.LastName, user.LastName));

        return principal;
    }
}
