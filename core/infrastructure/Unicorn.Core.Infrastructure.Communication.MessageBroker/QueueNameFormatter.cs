using System.Reflection;

namespace Unicorn.Core.Infrastructure.Communication.MessageBroker;

public static class QueueNameFormatter
{
    public static string GetNamespaceBasedName(MethodInfo method) => $"{method.DeclaringType!.FullName}.{method.Name}";
}
