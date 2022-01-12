using MediatR;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Handlers.Responses;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Handlers;

public abstract class BaseHandler<TRequest, TResponse> : OperationResults<TResponse>, IRequestHandler<TRequest, OperationResult<TResponse>>
    where TRequest : IRequest<OperationResult<TResponse>>
{
    public async Task<OperationResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken) =>
        await HandleAsync(request, cancellationToken);

    protected abstract Task<OperationResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
