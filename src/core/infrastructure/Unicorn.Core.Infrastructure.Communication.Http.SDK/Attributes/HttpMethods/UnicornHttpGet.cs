namespace Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UnicornHttpGetAttribute : UnicornHttpAttribute
{
    public UnicornHttpGetAttribute(string pathTemplate)
        : base(pathTemplate)
    {
    }
}
