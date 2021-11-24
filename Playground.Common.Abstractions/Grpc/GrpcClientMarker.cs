namespace Playground.Common.SDK.Abstractions;

[AttributeUsage(AttributeTargets.Interface)]
public class GrpcClientMarker : Attribute
{
    public GrpcClientMarker()
    {
    }
}
