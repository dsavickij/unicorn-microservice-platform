namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.MethodAttributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UnicornHttpPutAttribute : UnicornHttpAttribute
{
    public UnicornHttpPutAttribute(string pathTemplate)
        : base(pathTemplate)
    {
    }
}
