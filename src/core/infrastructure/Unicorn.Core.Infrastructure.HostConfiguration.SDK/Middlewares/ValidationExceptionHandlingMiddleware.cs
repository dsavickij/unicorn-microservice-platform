using FluentValidation;
using Microsoft.AspNetCore.Http;
using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.Middlewares;

internal class ValidationExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)OperationStatusCode.Status400BadRequest;
            var errors = ex.Errors.Select(x => new OperationError(GetCode(x.ErrorCode), x.ErrorMessage));

            await context.Response.WriteAsJsonAsync(new OperationResult(OperationStatusCode.Status400BadRequest, errors));
        }
    }

    private OperationStatusCode GetCode(string errorCode)
    {
        return Enum.TryParse<OperationStatusCode>(errorCode, out var result)
            ? result
            : OperationStatusCode.Status400BadRequest;
    }
}
