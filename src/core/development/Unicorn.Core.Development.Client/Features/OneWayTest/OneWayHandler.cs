using Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;

namespace Unicorn.Core.Development.Client.Features.OneWayTest;

public class OneWayHandler : BaseHandler.WithoutResult.ForRequest<OneWayRequest>
{
    protected override async Task HandleAsync(OneWayRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
