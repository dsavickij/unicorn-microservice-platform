using MediatR;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

public static class BaseRequest
{
    public abstract record WithResponse<TResponse> : IRequest<OperationResult<TResponse>>
        where TResponse : class
    {
    }

    public abstract record WithoutResponse : IRequest
    {
    }
}
