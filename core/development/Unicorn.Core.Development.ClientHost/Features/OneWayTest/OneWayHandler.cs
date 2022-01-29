using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

namespace Unicorn.Core.Development.ClientHost.Features.OneWayTest;

public class OneWayHandler : BaseHandler.WithoutResult.AfterExecutionOf<OneWayRequest>
{
    protected override async Task HandleAsync(OneWayRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
