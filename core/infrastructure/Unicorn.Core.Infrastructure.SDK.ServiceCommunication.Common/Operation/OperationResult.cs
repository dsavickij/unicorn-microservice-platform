using System.Text.Json.Serialization;

namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Common.Operation;

public record OperationResult : BaseOperationResult
{
    public OperationResult(OperationStatusCode code) : base(code)
    {
    }

    public OperationResult(OperationStatusCode code, OperationError error) : base(code, error)
    {
    }

    [JsonConstructor]
    public OperationResult(OperationStatusCode code, IEnumerable<OperationError> errors) : base(code, errors)
    {
    }
}

public record OperationResult<T> : BaseOperationResult
{
    public OperationResult(OperationStatusCode code, T data) : base(code) => Data = data;

    public OperationResult(OperationStatusCode code, OperationError error) : base(code, error)
    {
    }

    [JsonConstructor]
    public OperationResult(OperationStatusCode code, IEnumerable<OperationError> errors) : base(code, errors)
    {
    }

    public OperationResult(OperationStatusCode code) : base(code)
    {
    }

    [JsonInclude]
    public T? Data { get; private set; }

    public static implicit operator OperationResult<T>(OperationResult result) => new(result.Code, result.Errors);
}
