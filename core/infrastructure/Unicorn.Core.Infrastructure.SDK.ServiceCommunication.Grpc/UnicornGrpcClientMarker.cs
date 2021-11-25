namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc;

[AttributeUsage(AttributeTargets.Interface)]
public class UnicornGrpcClientMarker : Attribute
{
    public UnicornGrpcClientMarker()
    {
    }
}
