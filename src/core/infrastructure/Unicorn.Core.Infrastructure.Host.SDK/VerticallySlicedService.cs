using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;

namespace Unicorn.Core.Infrastructure.Host.SDK;

public abstract class VerticallySlicedService
{
    private readonly IMediator _mediator;

    protected VerticallySlicedService(IServiceProvider serviceProvider) =>
        _mediator = serviceProvider.GetRequiredService<IMediator>();

    protected async Task<OperationResult<TResponse>> SendAsync<TResponse>(IRequest<OperationResult<TResponse>> request)
        => await _mediator.Send(request);

    protected async Task<OperationResult> SendAsync(IRequest<OperationResult> request)
        => await _mediator.Send(request);

    protected async Task SendAsync(IRequest request) => await _mediator.Send(request);
}