using MediatR;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;

namespace Unicorn.Core.Infrastructure.Host.SDK.MediatR.Components;

public static class BaseHandler
{
    public static class WithResult<TResponse>
        where TResponse : notnull
    {
        /// <summary>
        /// Handler returning result of type TResponse after request execution
        /// </summary>
        /// <typeparam name="TResponse">.</typeparam>
        public abstract class ForRequest<TRequest> : BaseOperationResults<TResponse>, IRequestHandler<TRequest, OperationResult<TResponse>>
            where TRequest : notnull, IRequest<OperationResult<TResponse>>
        {
            public async Task<OperationResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
            {
                return await HandleAsync(request, cancellationToken);
            }

            protected abstract Task<OperationResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
        }
    }

    public static class WithResult
    {
        /// <summary>
        /// Handler returning result of type TResponse after request execution
        /// </summary>
        /// <typeparam name="TResponse">.</typeparam>
        public abstract class ForRequest<TRequest> : BaseOperationResults, IRequestHandler<TRequest, OperationResult>
            where TRequest : notnull, IRequest<OperationResult>
        {
            public async Task<OperationResult> Handle(TRequest request, CancellationToken cancellationToken)
            {
                return await HandleAsync(request, cancellationToken);
            }

            protected abstract Task<OperationResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
        }
    }

    public static class WithoutResult
    {
        /// <summary>
        /// Handler with no result to return after request execution
        /// </summary>
        public abstract class ForRequest<TRequest> : BaseOperationResults, IRequestHandler<TRequest>
            where TRequest : notnull, IRequest
        {
            public async Task Handle(TRequest request, CancellationToken cancellationToken)
            {
                await HandleAsync(request, cancellationToken);
            }

            protected abstract Task HandleAsync(TRequest request, CancellationToken cancellationToken);
        }
    }
}
