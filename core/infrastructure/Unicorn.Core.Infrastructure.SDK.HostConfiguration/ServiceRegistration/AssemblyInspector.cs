using System.Reflection;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration;

internal static class AssemblyInspector
{
    public static IEnumerable<string> GetServiceInterfaceNamesWithAttribute<TType>()
    {
        var attributeName = typeof(TType).AssemblyQualifiedName;
        var interfaceNames = new List<string>();

        using var ctx = MetadataLoadContextProvider.GetMedataLoadContext();

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
}
