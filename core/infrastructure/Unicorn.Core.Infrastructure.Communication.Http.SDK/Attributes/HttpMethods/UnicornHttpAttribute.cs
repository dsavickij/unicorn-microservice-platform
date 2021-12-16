namespace Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;

[AttributeUsage(AttributeTargets.Method)]
public abstract class UnicornHttpAttribute : Attribute
{
    protected UnicornHttpAttribute(string pathTemplate) => PathTemplate = pathTemplate;

    public string PathTemplate { get; }
}
