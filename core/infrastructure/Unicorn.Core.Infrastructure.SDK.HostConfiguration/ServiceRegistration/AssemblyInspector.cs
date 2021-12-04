using System.Reflection;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration;

internal static class AssemblyInspector
{
    public static IEnumerable<string> GetInterfaceNamesWithAttribute<TAttribute>()
        where TAttribute : Attribute
    {
        var attributeName = typeof(TAttribute).AssemblyQualifiedName;
        var interfaceNames = new List<string>();

        using var ctx = GetMedataLoadContext();

        foreach (var file in GetAssemblyFilesFromCurrentDirectory())
        {
            var assembly = ctx.LoadFromAssemblyPath(file);

            if (IsUnicornAssembly(assembly))
            {
                var names = assembly
                    .GetExportedTypes()
                    .Where(t => t.IsInterface && t.GetCustomAttributesData().Any(
                        data => data.AttributeType.AssemblyQualifiedName == attributeName))
                    .Select(t => t.AssemblyQualifiedName ?? string.Empty);

                interfaceNames.AddRange(names);
            }
        }

        return interfaceNames;
    }

    private static bool IsUnicornAssembly(Assembly assembly) =>
        assembly.FullName!.Contains("Unicorn", StringComparison.OrdinalIgnoreCase);

    private static IEnumerable<string> GetAssemblyFilesFromCurrentDirectory()
    {
        var path = Path.GetDirectoryName(typeof(AssemblyInspector).Assembly.Location) ?? string.Empty;
        return Directory.GetFiles(path).Where(fileName => fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));
    }

    private static MetadataLoadContext GetMedataLoadContext()
    {
        // Important: all assemblies (besides current and core assemblies) of referenced projects must be added or exception
        // will be thrown about not loaded assembly. Right now only 2 projects are referenced:
        // Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc and
        // Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http

        var currentAssembly = typeof(AssemblyInspector).Assembly;
        var coreAssembly = typeof(object).Assembly;
        var httpServiceMarkerAssembly = typeof(UnicornHttpServiceMarkerAttribute).Assembly;
        var grpcClientMarkerAssembly = typeof(UnicornGrpcClientMarkerAttribute).Assembly;

        var filePathes = new[]
        {
            currentAssembly.Location,
            coreAssembly.Location,
            httpServiceMarkerAssembly.Location,
            grpcClientMarkerAssembly.Location
        };

        return new MetadataLoadContext(new PathAssemblyResolver(filePathes), coreAssembly.GetName().Name);
    }
}
