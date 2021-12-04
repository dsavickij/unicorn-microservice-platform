namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http.Attributes.HttpMethods;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UnicornHttpPostAttribute : UnicornHttpAttribute
{
    public UnicornHttpPostAttribute(string pathTemplate)
        : base(pathTemplate)
    {
    }
}
