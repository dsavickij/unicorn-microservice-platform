using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Unicorn.Core.Services.Authentication.OpenIddict.Models;
using Unicorn.Core.Services.Authentication.OpenIddict.Services;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Unicorn.Core.Services.Authentication.OpenIddict;

public class Startup
{
    public Startup(IConfiguration configuration)
        => Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // Configure the context to use Microsoft SQL Server.
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            options.UseSqlite("Data Source=OpenIddictDb.db");

            // Register the entity sets needed by OpenIddict.
            // Note: use the generic overload if you need
            // to replace the default OpenIddict entities.
            options.UseOpenIddict();
        });

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        // Configure Identity to use the same JWT claims as OpenIddict instead
        // of the legacy WS-Federation claims it uses by default (ClaimTypes),
        // which saves you from doing the mapping in your authorization controller.
        services.Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.UserNameClaimType = Claims.Name;
            options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
            options.ClaimsIdentity.RoleClaimType = Claims.Role;
        });

        // OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
        // (like pruning orphaned authorizations/tokens from the database) at regular intervals.
        services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });

        // Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
                options.UseEntityFrameworkCore()
                   .UseDbContext<ApplicationDbContext>();

                // Enable Quartz.NET integration.
                options.UseQuartz();
            })

            // Register the OpenIddict server components.
            .AddServer(options =>
            {
                options.Configure(opt =>
                {
                    opt.TokenValidationParameters.ValidIssuers = new[]
                    {
                        "http://localhost:8001",
                        "https://localhost:8003",
                        "https://unicorn.core.services.authentication.openiddict:443",
                        "https://host.docker.internal:8003",
                        "https://host.docker.internal:44319",
                        "https://host.docker.internal"
                    };
                });

                options.DisableAccessTokenEncryption();
                // Enable the authorization, logout, userinfo, and introspection endpoints.
                options.SetAuthorizationEndpointUris("/connect/authorize")
                   .SetEndSessionEndpointUris("/connect/logout")
                   .SetIntrospectionEndpointUris("/connect/introspect")
                   .SetUserInfoEndpointUris("/connect/userinfo")
                   .SetTokenEndpointUris("/connect/token");

                // Mark the "email", "profile" and "roles" scopes as supported scopes.
                options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

                // Note: the sample only uses the implicit flow but you can enable the other
                // flows if you need to support code, password or client credentials.
                options
                .AllowImplicitFlow()
                .AllowClientCredentialsFlow();

                // Register the encryption credentials. This sample uses a symmetric
                // encryption key that is shared between the server and the Api2 sample
                // (that performs local token validation instead of using introspection).
                //
                // Note: in a real world application, this encryption key should be
                // stored in a safe place (e.g in Azure KeyVault, stored as a secret).
                options.AddEncryptionKey(new SymmetricSecurityKey(
                Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));

                // Register the signing credentials.
                options.AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                options.UseAspNetCore()
                   .EnableAuthorizationEndpointPassthrough()
                   .EnableEndSessionEndpointPassthrough()
                   .EnableUserInfoEndpointPassthrough()
                   .EnableStatusCodePagesIntegration()
                   .EnableTokenEndpointPassthrough();

                //---------
                // Enable the token endpoint.
                //options.SetTokenEndpointUris("/connect/token");

                //// Enable the client credentials flow.
                //options.AllowClientCredentialsFlow();

                //// Register the signing and encryption credentials.
                //options.AddDevelopmentEncryptionCertificate()
                //   .AddDevelopmentSigningCertificate();

                //// Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                //options.UseAspNetCore()
                //   .EnableTokenEndpointPassthrough();
            })

            // Register the OpenIddict validation components.
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();
   
                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });

        services.AddCors();
        services.AddControllersWithViews();

        services.AddTransient<IEmailSender, AuthMessageSender>();
        services.AddTransient<ISmsSender, AuthMessageSender>();

        services.AddTransient<IUserClaimsPrincipalFactory<ApplicationUser>, UnicornUserClaimsPrincipalFactory>();

        // Register the worker responsible of seeding the database with the sample clients.
        // Note: in a real world application, this step should be part of a setup script.
        services.AddHostedService<Worker>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseStatusCodePagesWithReExecute("/error");

        app.UseRouting();

        app.UseCors(builder =>
        {
            builder.WithOrigins("https://localhost:44398");
            builder.WithMethods("GET");
            builder.WithHeaders("Authorization");
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(options =>
        {
            options.MapControllers();
            options.MapDefaultControllerRoute();
        });
    }
}
