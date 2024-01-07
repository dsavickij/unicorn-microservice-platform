namespace Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UnicornHttpPostAttribute : UnicornHttpAttribute
{
    public UnicornHttpPostAttribute(string pathTemplate)
        : base(pathTemplate)
    {
    }
}
