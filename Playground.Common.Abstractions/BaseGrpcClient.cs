namespace Playground.Common.SDK.Abstractions;

public abstract class BaseGrpcClient
{
    public BaseGrpcClient(IGrpcClientFactory factory) => Factory = factory;

    public IGrpcClientFactory Factory { get; }

    protected abstract string GrpcServiceName { get; }
}