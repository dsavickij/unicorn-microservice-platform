using MediatR;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

public static class BaseHandler
{
    public static class WithResult<TResponse>
        where TResponse : class
    {
        /// <summary>
        /// Handler returning result of type TResponse after request execution
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        public abstract class For<TRequest> : OperationResults<TResponse>, IRequestHandler<TRequest, OperationResult<TResponse>>
            where TRequest : IRequest<OperationResult<TResponse>>
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
        /// <typeparam name="TResponse"></typeparam>
        public abstract class For<TRequest> : OperationResults, IRequestHandler<TRequest, OperationResult>
            where TRequest : IRequest<OperationResult>
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
        public abstract class For<TRequest> : OperationResults, IRequestHandler<TRequest, Unit>
            where TRequest : IRequest
        {
            public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
            {
                await HandleAsync(request, cancellationToken);
                return Unit.Value;
            }

            protected abstract Task HandleAsync(TRequest request, CancellationToken cancellationToken);
        }
    }
}
