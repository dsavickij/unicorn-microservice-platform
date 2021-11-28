namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UnicornHttpDeleteAttribute : UnicornHttpAttribute
{
    public UnicornHttpDeleteAttribute(string pathTemplate)
        : base(pathTemplate)
    {
    }
}
