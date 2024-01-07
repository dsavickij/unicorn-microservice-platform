using Unicorn.Core.Infrastructure.Communication.Common.Operation;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;

namespace Unicorn.Core.Infrastructure.Communication.Grpc.SDK;

public abstract class BaseGrpcClient : OperationResults
{
    protected BaseGrpcClient(IGrpcServiceClientFactory factory) => Factory = factory;

    public IGrpcServiceClientFactory Factory { get; }
}
