namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UnicornHttpGetAttribute : UnicornHttpAttribute
{
    public UnicornHttpGetAttribute(string pathTemplate)
        : base(pathTemplate)
    {
    }
}
