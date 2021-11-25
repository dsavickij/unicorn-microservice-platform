namespace Playground.Core.Infrastructure.SDK.ServiceCommunication.Http;

[AttributeUsage(AttributeTargets.Interface)]
public class PlaygroundHttpServiceMarker : Attribute
{
    public PlaygroundHttpServiceMarker()
    {
    }
}
