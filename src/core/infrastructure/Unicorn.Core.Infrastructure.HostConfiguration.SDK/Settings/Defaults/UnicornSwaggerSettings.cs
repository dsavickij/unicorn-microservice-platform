using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.Settings.Defaults;

internal static class UnicornSwaggerSettings
{
    internal static SwaggerUIOptions UIOptions => new()
    {
        OAuthConfigObject = new OAuthConfigObject
        {
            Realm = "client-realm",
            AppName = "OAuth-app",
            UseBasicAuthenticationWithAccessCodeGrant = true,
            ClientId = "aurelia"
        },
        ConfigObject = new ConfigObject
        {
            Urls = new List<UrlDescriptor>
            {
                new() { Name = "JWTAuthDemo v1", Url = "/swagger/v1/swagger.json" }
            }
        }
    };

    internal static Action<SwaggerGenOptions> GetSwaggerGenOptions(string auhorityUrl)
    {
        return opt =>
        {
            // opt.SwaggerDoc("My Swagger", new OpenApiInfo { Title = "My Title", Version = "v1" });
            var securityScheme = new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                Name = "Authorization",
                BearerFormat = "JWT",
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(new Uri(auhorityUrl), "/connect/authorize")
                    }
                },
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            };

            opt.AddSecurityDefinition("oauth2", securityScheme);

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() }
            });
        };
    }
}
