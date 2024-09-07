using MediatR;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

public static class BaseRequest
{
    public abstract record RequiringResult<TResponse> : IRequest<OperationResult<TResponse>>
        where TResponse : notnull
    {
    }

    public abstract record RequiringResult : IRequest<OperationResult>
    {
    }

    public abstract record RequiringNoResult : IRequest
    {
    }
}