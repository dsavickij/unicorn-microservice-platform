using MediatR;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Handlers.Responses;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Handlers;

public abstract class BaseOneWayHandler<TRequest> : OperationResults, IRequestHandler<TRequest, Unit>
    where TRequest : IRequest
{
    public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await HandleAsync(request, cancellationToken);
        return Unit.Value;
    }

    protected abstract Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}
