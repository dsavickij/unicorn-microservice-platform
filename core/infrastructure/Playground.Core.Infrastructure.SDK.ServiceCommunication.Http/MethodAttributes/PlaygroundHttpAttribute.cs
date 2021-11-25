namespace Playground.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;

public abstract class PlaygroundHttpAttribute : Attribute
{
    public PlaygroundHttpAttribute(string pathTemplate)
    {
        PathTemplate = pathTemplate;
    }

    public string PathTemplate { get; }
}
