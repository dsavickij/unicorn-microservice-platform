using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Unicorn.Core.Infrastructure.Security.IAM.AuthenticationContext;

namespace Unicorn.Core.Infrastructure.Security.IAM.Middlewares;

internal class UnicornOperationContextMiddleware
{
    private readonly RequestDelegate _next;

    public UnicornOperationContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.User.Identity?.IsAuthenticated is true)
        {
            var accessToken = GetAccessToken(httpContext.Request.Headers);
            var identity = new UnicornIdentity(accessToken, httpContext.User.Claims);

            UnicornOperationContext.Set(identity);
        }

        await _next(httpContext);
    }

    private string GetAccessToken(IHeaderDictionary headers)
    {
        return headers[HeaderNames.Authorization].ToString()
            .Replace("Bearer ", string.Empty, StringComparison.OrdinalIgnoreCase);
    }
}