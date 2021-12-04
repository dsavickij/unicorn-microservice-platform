namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.Attributes.HttpMethods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UnicornHttpDeleteAttribute : UnicornHttpAttribute
{
    public UnicornHttpDeleteAttribute(string pathTemplate)
        : base(pathTemplate)
    {
    }
}
