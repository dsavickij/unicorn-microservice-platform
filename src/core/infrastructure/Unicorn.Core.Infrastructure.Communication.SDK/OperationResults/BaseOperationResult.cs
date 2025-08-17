using System.Diagnostics.CodeAnalysis;

namespace Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;

public interface IOperationResult
{
    IEnumerable<OperationError> Errors { get; }
    bool IsSuccess { get; }
    OperationStatusCode Code { get; }
}

public abstract record BaseOperationResult : IOperationResult
{
    protected BaseOperationResult(OperationStatusCode code, OperationError error) : this(code, new[] { error })
    {
    }

    protected BaseOperationResult(OperationStatusCode code, IEnumerable<OperationError> errors) : this(code)
    {
        Errors = errors;
    }

    protected BaseOperationResult(OperationStatusCode code)
    {
        Code = code;
        IsSuccess = code is >= OperationStatusCode.Status200OK and < OperationStatusCode.Status300MultipleChoices;
    }

    public IEnumerable<OperationError> Errors { get; } = [];
    
    public bool IsSuccess { get; }

    public OperationStatusCode Code { get; }
}
