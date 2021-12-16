using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;

namespace Unicorn.Core.Infrastructure.Communication.Grpc.SDK;

public abstract class BaseGrpcClient
{
    protected BaseGrpcClient(IGrpcServiceClientFactory factory) => Factory = factory;

    public IGrpcServiceClientFactory Factory { get; }

    protected abstract string GrpcServiceName { get; }
}
