using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using Unicorn.Core.Infrastructure.Security.IAM.AuthenticationScope;
using Unicorn.Core.Infrastructure.Security.IAM.Settings;

namespace Unicorn.Core.Infrastructure.Security.IAM;

public static class AuthenticationConfigurationExtensions
{
    public static void ConfigureAuthentication(this IServiceCollection services, AuthenticationSettings authenticationSettings)
    {
        services.AddTransient<IAuthenticationScope, UnicornAuthenticationScope>();
        services.AddTransient<ITokenManager, TokenManager>();

        services.ConfigureOpenIddct(authenticationSettings);
    }

    private static void ConfigureOpenIddct(this IServiceCollection services, AuthenticationSettings authenticationSettings)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        // Register the OpenIddict validation components.
        services
            .AddOpenIddict();
            //.AddValidation(options =>
            //{
            //    // Note: the validation handler uses OpenID Connect discovery
            //    // to retrieve the issuer signing keys used to validate tokens.
            //    options.SetIssuer(authenticationSettings.AuthorityUrl);
            //    // options.UseIntrospection()
            //    //  .SetClientId(cfg.ClientCredentials.ClientId)
            //    //  .SetClientSecret(cfg.ClientCredentials.ClientSecret);

            //    // Register the encryption credentials. This sample uses a symmetric
            //    // encryption key that is shared between the server and the Api2 sample
            //    // (that performs local token validation instead of using introspection).
            //    //
            //    // Note: in a real world application, this encryption key should be
            //    // stored in a safe place (e.g in Azure KeyVault, stored as a secret).
            //    options.AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));

            //    // Register the System.Net.Http integration.
            //    options.UseSystemNetHttp();

            //    // Register the ASP.NET Core host.
            //    options.UseAspNetCore();
            //});
    }
}
