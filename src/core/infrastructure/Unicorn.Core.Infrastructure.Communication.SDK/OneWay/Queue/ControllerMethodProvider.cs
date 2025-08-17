using System.Reflection;

namespace Unicorn.Core.Infrastructure.Communication.SDK.OneWay.Queue;

internal interface IControllerMethodProvider
{
    MethodInfo GetOneWayMethod(string methodName, int numberOfParameters);
}

internal class ControllerMethodProvider : IControllerMethodProvider
{
    private readonly ILookup<string, MethodInfo> _methods;

    public ControllerMethodProvider(IEnumerable<MethodInfo> methods) => _methods = methods.ToLookup(x => x.Name);

    public MethodInfo GetOneWayMethod(string methodName, int numberOfParameters)
    {
        var method = _methods[methodName].FirstOrDefault(x => x.GetParameters().Length == numberOfParameters);
        return method ?? throw new ArgumentNullException($"Method with name '{methodName}' is not registered");
    }
}
