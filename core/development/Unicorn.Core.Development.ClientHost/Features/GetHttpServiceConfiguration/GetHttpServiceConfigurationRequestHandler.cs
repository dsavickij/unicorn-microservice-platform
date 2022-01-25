﻿using OneOf;
using OneOf.Types;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Handlers;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.ClientHost.Features.GetHttpServiceConfiguration;

public class GetHttpServiceConfigurationRequestHandler : BaseHandler<GetHttpServiceConfigurationRequest, HttpServiceConfiguration>
{
    private readonly ILogger<GetHttpServiceConfigurationRequestHandler> _logger;

    public GetHttpServiceConfigurationRequestHandler(ILogger<GetHttpServiceConfigurationRequestHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<OperationResult<HttpServiceConfiguration>> HandleAsync(
        GetHttpServiceConfigurationRequest request, CancellationToken cancellationToken)
    {
        return HandleRequestAsync(request).Match(
          cfg => Ok(cfg),
          none => NotFound(),
          noAuthorization => Forbidden(),
          dependencyFailure => BadRequest(dependencyFailure.OperationResult.Errors));
    }

    private OneOf<HttpServiceConfiguration, None, NoAuthorization, DependencyFailure> HandleRequestAsync(
        GetHttpServiceConfigurationRequest request)
    {
        var services = new[]
        {
            new HttpServiceConfiguration { Name = "test" },
            new HttpServiceConfiguration { Name = "test2" }
        };

        if (2 == 3)
        {
            return new NoAuthorization();
        }

        var cfg = services.FirstOrDefault(s => s.Name.Equals(request.ServiceName));

        return cfg is null ? new None() : cfg;
    }
}

public record struct NoAuthorization { }
public record struct DependencyFailure(OperationResult OperationResult) { }