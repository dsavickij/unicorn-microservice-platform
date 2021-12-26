using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK;

[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= Guard.Against.Null(
        HttpContext.RequestServices.GetService<IMediator>()!, $"'{nameof(IMediator)}' failed to be resolved");

    protected async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        where TResponse : new() => await Mediator.Send(request);

    protected async Task SendAsync(IRequest request) => await Mediator.Send(request);
}
