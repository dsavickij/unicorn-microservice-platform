using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.Middlewares;

internal class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionLoggingMiddleware> _logger;

    public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var msg = GetErrorMessage(ex);
            _logger.LogError(ex, message: msg, Array.Empty<object>());

            throw;
        }
    }

    private string GetErrorMessage(Exception ex)
    {
        return ex is AggregateException aex
            ? string.Join("; ", aex.Flatten().InnerExceptions.Select(x => x.Message))
            : ex.Message;
    }
}
