using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc.Contracts;

namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc;

public abstract class BaseGrpcClient
{
    protected BaseGrpcClient(IGrpcServiceClientFactory factory) => Factory = factory;

    public IGrpcServiceClientFactory Factory { get; }

    protected abstract string GrpcServiceName { get; }
}
