namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.Attributes.HttpMethods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UnicornHttpPutAttribute : UnicornHttpAttribute
{
    public UnicornHttpPutAttribute(string pathTemplate)
        : base(pathTemplate)
    {
    }
}
