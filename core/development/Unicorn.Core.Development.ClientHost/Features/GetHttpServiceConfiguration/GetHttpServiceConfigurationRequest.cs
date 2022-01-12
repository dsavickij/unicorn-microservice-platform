using MediatR;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Services.ServiceDiscovery.SDK.Configurations;

namespace Unicorn.Core.Development.ClientHost.Features.GetHttpServiceConfiguration;

public record GetHttpServiceConfigurationRequest : IRequest<OperationResult<HttpServiceConfiguration>>
{
    public string ServiceName { get; set; } = string.Empty;
}
