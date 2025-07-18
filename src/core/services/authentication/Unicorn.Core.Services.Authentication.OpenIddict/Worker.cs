﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using Unicorn.Core.Services.Authentication.OpenIddict.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Unicorn.Core.Services.Authentication.OpenIddict;

public class Worker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.EnsureCreatedAsync();

        await CreateApplicationsAsync();
        await CreateScopesAsync();

        async Task CreateApplicationsAsync()
        {
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("aurelia") == null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = "aurelia",
                    DisplayName = "Aurelia client application",
                    PostLogoutRedirectUris =
                    {
                        new Uri("https://localhost:44398/signout-oidc"),
                    },
                    RedirectUris =
                    {
                        new Uri("https://localhost:44398/signin-oidc"),
                        new Uri("http://localhost:8000/swagger/oauth2-redirect.html"),
                        new Uri("https://localhost:8001/swagger/oauth2-redirect.html"),
                        new Uri("https://localhost:7001/swagger/oauth2-redirect.html"),
                        new Uri("https://host.docker.internal:8001/swagger/oauth2-redirect.html")
                    },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.EndSession,
                        Permissions.GrantTypes.Implicit,
                        Permissions.ResponseTypes.IdToken,
                        Permissions.ResponseTypes.IdTokenToken,
                        Permissions.ResponseTypes.Token,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "api1",
                        Permissions.Prefixes.Scope + "api2"
                    }
                };

                await manager.CreateAsync(descriptor);
            }

            if (await manager.FindByClientIdAsync("resource_server_1") == null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = "resource_server_1",
                    ClientSecret = "846B62D0-DEF9-4215-A99D-86E6B8DAB342",
                    Permissions =
                    {
                        Permissions.Endpoints.Introspection,

                        // these for testing
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.ClientCredentials
                    }
                };

                await manager.CreateAsync(descriptor);
            }

            // Note: no client registration is created for resource_server_2
            // as it uses local token validation instead of introspection.

            if (await manager.FindByClientIdAsync("console") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "console",
                    ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                    DisplayName = "My client application",
                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.ClientCredentials
                    }
                });
            }
        }

        async Task CreateScopesAsync()
        {
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

            if (await manager.FindByNameAsync("api1") == null)
            {
                var descriptor = new OpenIddictScopeDescriptor
                {
                    Name = "api1",
                    Resources =
                    {
                        "resource_server_1"
                    }
                };

                await manager.CreateAsync(descriptor);
            }

            if (await manager.FindByNameAsync("api2") == null)
            {
                var descriptor = new OpenIddictScopeDescriptor
                {
                    Name = "api2",
                    Resources =
                    {
                        "resource_server_2"
                    }
                };

                await manager.CreateAsync(descriptor);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
