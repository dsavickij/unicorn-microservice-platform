using System.Reflection;

namespace Playground.Core.Infrastructure.SDK.HostConfiguration.Common;

internal class AssemblyInspector
{
    public static IEnumerable<string> GetServiceInterfaceNamesWithAttributeOfType<T>()
    {
        var attributeName = typeof(T).AssemblyQualifiedName;
        var interfaceNames = new List<string>();

        using var ctx = MetadataLoadContextProvider.GetMedataLoadContext();

        foreach (var file in GetAssemblyFilesFromCurrentDirectory())
        {
            var assembly = ctx.LoadFromAssemblyPath(file);

            if (IsAssemblyPlaygroundServiceAssembly(assembly))
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

    private static bool IsAssemblyPlaygroundServiceAssembly(Assembly assembly) => 
        assembly.FullName!.Contains("Playground");

    private static IEnumerable<string> GetAssemblyFilesFromCurrentDirectory()
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        return Directory.GetFiles(path).Where(fileName => fileName.EndsWith(".dll"));
    }
}
