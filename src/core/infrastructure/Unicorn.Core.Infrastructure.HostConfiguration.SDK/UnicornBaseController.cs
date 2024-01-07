using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK;

[ApiController]
public abstract class UnicornHttpService<TUnicornHttpServiceInterface> : ControllerBase
    where TUnicornHttpServiceInterface : class
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= Guard.Against.Null(
        HttpContext.RequestServices.GetService<IMediator>()!, $"'{nameof(IMediator)}' failed to be resolved");

    protected async Task<OperationResult<TResponse>> SendAsync<TResponse>(IRequest<OperationResult<TResponse>> request)
        => await Mediator.Send(request);

    protected async Task<OperationResult> SendAsync(IRequest<OperationResult> request)
        => await Mediator.Send(request);

    protected async Task SendAsync(IRequest request) => await Mediator.Send(request);
}
