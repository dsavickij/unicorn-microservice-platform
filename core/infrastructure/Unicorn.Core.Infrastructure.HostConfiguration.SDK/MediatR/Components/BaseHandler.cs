﻿using MediatR;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

public static class BaseHandler
{
    public static class WithResultOf<TResponse>
        where TResponse : class
    {
        /// <summary>
        /// Handler returning result of type TResponse after request execution
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        public abstract class AfterExecutionOf<TRequest> : OperationResults<TResponse>, IRequestHandler<TRequest, OperationResult<TResponse>>
            where TRequest : IRequest<OperationResult<TResponse>>
        {
            public async Task<OperationResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
            {
                return await HandleAsync(request, cancellationToken);
            }

            protected abstract Task<OperationResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
        }
    }

    public static class WithoutResult
    {
        /// <summary>
        /// Handler with no result to return after request execution
        /// </summary>
        public abstract class AfterExecutionOf<TRequest> : OperationResults, IRequestHandler<TRequest, Unit>
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