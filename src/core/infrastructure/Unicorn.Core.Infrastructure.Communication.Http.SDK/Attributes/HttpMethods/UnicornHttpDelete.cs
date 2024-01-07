namespace Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UnicornHttpDeleteAttribute : UnicornHttpAttribute
{
    public UnicornHttpDeleteAttribute(string pathTemplate)
        : base(pathTemplate)
    {
    }
}
