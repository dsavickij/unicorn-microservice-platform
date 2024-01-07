namespace Unicorn.Core.Infrastructure.Communication.Common.Operation;

public record OperationError(OperationStatusCode Code, string Message)
{
}
