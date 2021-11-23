namespace Playground.Common.SDK.Abstractions.Http.MethodAttributes;

public abstract class PlaygroundHttpAttribute : Attribute
{
    public PlaygroundHttpAttribute(string pathTemplate)
    {
        PathTemplate = pathTemplate;
    }

    public string PathTemplate { get; }
}
