using OneOf;
using OneOf.Types;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.Client.Features.GetHttpServiceConfiguration;

public class GetHttpServiceConfigurationRequestHandler : BaseHandler.WithResult<HttpServiceConfiguration>.ForRequest<GetHttpServiceConfigurationRequest>
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
            new HttpServiceConfiguration { ServiceHostName = "test" },
            new HttpServiceConfiguration { ServiceHostName = "test2" }
        };

        if (2 == 3)
        {
            return new NoAuthorization();
        }

        var cfg = services.FirstOrDefault(s => s.ServiceHostName.Equals(request.ServiceName));

        return cfg is null ? new None() : cfg;
    }
}

public record struct NoAuthorization { }
public record struct DependencyFailure(OperationResult OperationResult) { }

