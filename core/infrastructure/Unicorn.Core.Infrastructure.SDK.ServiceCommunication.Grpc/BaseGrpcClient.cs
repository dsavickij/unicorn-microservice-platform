using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc.Contracts;

namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc;

public abstract class BaseGrpcClient
{
    protected BaseGrpcClient(IGrpcClientFactory factory) => Factory = factory;

    public IGrpcClientFactory Factory { get; }

    protected abstract string GrpcServiceName { get; }
}
