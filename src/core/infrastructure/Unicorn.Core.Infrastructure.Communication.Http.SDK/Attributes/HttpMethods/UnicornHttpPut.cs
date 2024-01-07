namespace Unicorn.Core.Infrastructure.Communication.Http.SDK.Attributes.HttpMethods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UnicornHttpPutAttribute : UnicornHttpAttribute
{
    public UnicornHttpPutAttribute(string pathTemplate)
        : base(pathTemplate)
    {
    }
}
