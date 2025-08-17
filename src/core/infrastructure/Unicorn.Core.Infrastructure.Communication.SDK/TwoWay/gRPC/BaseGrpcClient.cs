using Unicorn.Core.Infrastructure.Communication.SDK.OperationResults;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.gRPC.Contracts;

namespace Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.gRPC;

public abstract class BaseGrpcClient : BaseOperationResults
{
    protected BaseGrpcClient(IGrpcServiceClientFactory factory) => Factory = factory;

    public IGrpcServiceClientFactory Factory { get; }
}
