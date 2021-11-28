namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UnicornHttpPostAttribute : UnicornHttpAttribute
{
    public UnicornHttpPostAttribute(string pathTemplate)
        : base(pathTemplate)
    {
    }
}
