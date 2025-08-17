namespace Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;

public record OperationError(OperationStatusCode Code, string Message)
{
}
