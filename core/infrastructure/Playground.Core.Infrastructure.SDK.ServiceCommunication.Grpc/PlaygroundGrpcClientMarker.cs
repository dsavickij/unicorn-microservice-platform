namespace Playground.Core.Infrastructure.SDK.ServiceCommunication.Grpc;

[AttributeUsage(AttributeTargets.Interface)]
public class PlaygroundGrpcClientMarker : Attribute
{
    public PlaygroundGrpcClientMarker()
    {
    }
}
