using MassTransit;
using System.Reflection;
using Unicorn.Core.Infrastructure.Communication.MessageBroker.Abstractions;

namespace Unicorn.Core.Infrastructure.Communication.MessageBroker;

internal static class AssemblyScanner
{
    private static readonly IEnumerable<Type> _eventHandlers = FindEventHandlers();

    public static IEnumerable<Type> GetEventHandlers() => _eventHandlers;

    private static IEnumerable<Type> FindEventHandlers()
    {
        var assemblyNames = Assembly.GetEntryAssembly()!
            .GetReferencedAssemblies()
            .Append(Assembly.GetEntryAssembly()!.GetName());

        var exportedTypes = assemblyNames
            .Select(x => Assembly.Load(x))
            .SelectMany(x => x.GetExportedTypes());

        return exportedTypes.Where(x => x.GetInterfaces().Any(
            y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IUnicornEventHandler<>)));
    }
}
