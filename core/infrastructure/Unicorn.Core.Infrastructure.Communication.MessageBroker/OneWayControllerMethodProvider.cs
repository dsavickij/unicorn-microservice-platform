using System.Reflection;

namespace Unicorn.Core.Infrastructure.Communication.MessageBroker;

public interface IOneWayControllerMethodProvider
{
    MethodInfo GetOneWayMethod(string methodName, int numberOfParameters);
}

public class OneWayControllerMethodProvider : IOneWayControllerMethodProvider
{
    private readonly ILookup<string, MethodInfo> _oneWayMethods;

    public OneWayControllerMethodProvider(IEnumerable<MethodInfo> oneWayMethods) => 
        _oneWayMethods = oneWayMethods.ToLookup(x => x.Name);

    public MethodInfo GetOneWayMethod(string methodName, int numberOfParameters)
    {
        var method = _oneWayMethods[methodName].FirstOrDefault(x => x.GetParameters().Length == numberOfParameters);
        return method ?? throw new ArgumentNullException($"Method with name '{methodName}' is not registered");
    }
}
