using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Handlers;

namespace Unicorn.Core.Development.ClientHost.Features.OneWayTest;

public class OneWayHandler : BaseOneWayHandler<OneWayRequest>
{
    protected override async Task HandleAsync(OneWayRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
