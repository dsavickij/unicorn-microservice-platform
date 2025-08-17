namespace Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.Rest;

/// <summary>
/// Attribute to set serviceName for all HTTP service interfaces in the assembly for configuration retrieval
/// from service discovery service
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public class UnicornServiceHostNameAttribute : Attribute
{
    public string ServiceHostName { get; }

    public UnicornServiceHostNameAttribute(string serviceHostName)
    {
        ServiceHostName = serviceHostName;
    }
}
