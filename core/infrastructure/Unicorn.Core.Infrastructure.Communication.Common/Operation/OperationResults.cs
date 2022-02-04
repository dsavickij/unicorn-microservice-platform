namespace Unicorn.Core.Infrastructure.Communication.Common.Operation;

public abstract class OperationResults
{
    protected OperationResult<TResponse> Ok<TResponse>(TResponse response) => new(OperationStatusCode.Status200OK, response);
    protected OperationResult Ok() => new(OperationStatusCode.Status200OK);
    protected OperationResult BadRequest(OperationError error) => BadRequest(new[] { error });
    protected OperationResult BadRequest(IEnumerable<OperationError> errors) => new(OperationStatusCode.Status400BadRequest, errors);
    protected OperationResult Forbidden() => new(OperationStatusCode.Status403Forbidden);
    protected OperationResult NotFound() => new(OperationStatusCode.Status404NotFound);
    protected OperationResult NotFound(string message) =>
        new(OperationStatusCode.Status404NotFound, new OperationError(OperationStatusCode.Status404NotFound, message));
}
