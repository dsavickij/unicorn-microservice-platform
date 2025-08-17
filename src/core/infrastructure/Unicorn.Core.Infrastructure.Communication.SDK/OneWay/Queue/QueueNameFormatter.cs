using System.Reflection;

namespace Unicorn.Core.Infrastructure.Communication.SDK.OneWay.Queue;

public static class QueueNameFormatter
{
    public static string GetNamespaceBasedName(MethodInfo method) => $"{method.DeclaringType!.FullName}.{method.Name}";
}
