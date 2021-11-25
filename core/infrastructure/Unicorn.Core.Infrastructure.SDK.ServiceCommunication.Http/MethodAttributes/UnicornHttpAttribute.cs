namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;

public abstract class UnicornHttpAttribute : Attribute
{
    public UnicornHttpAttribute(string pathTemplate)
    {
        PathTemplate = pathTemplate;
    }

    public string PathTemplate { get; }
}
