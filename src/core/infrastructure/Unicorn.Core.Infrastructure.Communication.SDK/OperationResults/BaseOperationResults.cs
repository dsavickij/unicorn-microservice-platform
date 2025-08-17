namespace Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;

public abstract class BaseOperationResults
{
    protected OperationResult Ok() => new(OperationStatusCode.Status200OK);
    protected OperationResult<TResponse> Ok<TResponse>(TResponse response) => new(OperationStatusCode.Status200OK, response);
    protected OperationResult BadRequest(OperationError error) => BadRequest(new[] { error });
    protected OperationResult BadRequest(IEnumerable<OperationError> errors) => new(OperationStatusCode.Status400BadRequest, errors);
    protected OperationResult Forbidden() => new(OperationStatusCode.Status403Forbidden);
    protected OperationResult NotFound() => new(OperationStatusCode.Status404NotFound);
    protected OperationResult NotFound(string message) =>
        new(OperationStatusCode.Status404NotFound, new OperationError(OperationStatusCode.Status404NotFound, message));
}

public abstract class BaseOperationResults<TResponse> : BaseOperationResults 
    where TResponse : notnull
{
    protected OperationResult<TResponse> Ok(TResponse response) => new(OperationStatusCode.Status200OK, response);
}

public static class OperationResults
{
    public static OperationResult Ok() => new(OperationStatusCode.Status200OK);
    public static OperationResult<TResponse> Ok<TResponse>(TResponse response) => new(OperationStatusCode.Status200OK, response);
    public static OperationResult BadRequest(OperationError error) => BadRequest([error]);
    public static OperationResult BadRequest(IEnumerable<OperationError> errors) => new(OperationStatusCode.Status400BadRequest, errors);
    public static OperationResult Forbidden() => new(OperationStatusCode.Status403Forbidden);
    public static OperationResult NotFound() => new(OperationStatusCode.Status404NotFound);
    public static OperationResult NotFound(string message) => new(OperationStatusCode.Status404NotFound, new OperationError(OperationStatusCode.Status404NotFound, message));

}