namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Common.Operation;

public record OperationError(OperationStatusCode Code, string Message)
{
}
