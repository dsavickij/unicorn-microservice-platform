using Unicorn.Core.Infrastructure.Communication.Common.Operation;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.MediatR.Components;

public abstract class OperationResults<TResponse> : OperationResults
{
    protected OperationResult<TResponse> Ok(TResponse response) => new(OperationStatusCode.Status200OK, response);
}

public abstract class OperationResults
{
    protected OperationResult Ok() => new (OperationStatusCode.Status200OK);
    protected OperationResult BadRequest(OperationError error) => BadRequest(new[] { error });
    protected OperationResult BadRequest(IEnumerable<OperationError> errors) => new (OperationStatusCode.Status400BadRequest, errors);
    protected OperationResult Forbidden() => new (OperationStatusCode.Status403Forbidden);
    protected OperationResult NotFound() => new (OperationStatusCode.Status404NotFound);
    protected OperationResult NotFound(string message) =>
        new (OperationStatusCode.Status404NotFound, new OperationError(OperationStatusCode.Status404NotFound, message));
}
