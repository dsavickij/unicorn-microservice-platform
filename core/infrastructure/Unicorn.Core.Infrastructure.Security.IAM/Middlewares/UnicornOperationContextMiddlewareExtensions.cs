using Microsoft.AspNetCore.Builder;

namespace Unicorn.Core.Infrastructure.Security.IAM.Middlewares;

public static class UnicornOperationContextMiddlewareExtensions
{
    public static void UseUnicornOperationContext(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<UnicornOperationContextMiddleware>();
    }
}
