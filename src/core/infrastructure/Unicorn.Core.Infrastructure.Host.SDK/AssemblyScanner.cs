using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Unicorn.Core.Infrastructure.Communication.SDK.OneWay;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.gRPC;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.Rest;

namespace Unicorn.Core.Infrastructure.Host.SDK;

internal static class AssemblyScanner
{
    public static IEnumerable<string> GetInterfaceNamesDecoratedWith<TAttribute>()
        where TAttribute : Attribute
    {
        var attributeName = typeof(TAttribute).AssemblyQualifiedName;
        var interfaceNames = new List<string>();

        using var ctx = GetMetadataLoadContext();

        foreach (var file in GetAssemblyFilesFromCurrentDirectory())
        {
            var assembly = ctx.LoadFromAssemblyPath(file);

            if (IsUnicornAssembly(assembly!))
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

    /// <summary>
    /// Loads host entry and referenced assemblies to be used in delegate (e.g. to do one-time registration in MediatR)
    /// </summary>
    /// <param name="assemblyUtilizer">Delegate to use loaded assemblies</param>
    public static void UseHostUnicornAssemblies(Action<Assembly[]> assemblyUtilizer)
    {
        var referencedAssemblies = Assembly.GetEntryAssembly()!
            .GetReferencedAssemblies()
            .Where(x => IsUnicornAssemblyName(x))
            .Select(x => Assembly.Load(x));

        var allAssemblies = new List<Assembly>(referencedAssemblies) { Assembly.GetEntryAssembly()! }.ToArray();

        assemblyUtilizer(allAssemblies);
    }

    public static IEnumerable<Type> GetUnicornControllers()
    {
        return Assembly.GetEntryAssembly()!.GetExportedTypes().Where(
            x => x.BaseType != null &&
            x.BaseType.IsGenericType &&
            x.BaseType.GetGenericTypeDefinition() == typeof(UnicornHttpService<>));
    }

    // TODO: check is this is needed
    // public static IEnumerable<OneWayMethodConfiguration> GetOneWayMethodConfigurations()
    // {
    //     var configurations = new List<OneWayMethodConfiguration>();
    //
    //     foreach (var controller in GetUnicornControllers())
    //     {
    //         var httpServiceInterfaceType = controller.BaseType!.GetGenericArguments()[0];
    //
    //         var interfaceMethods = httpServiceInterfaceType
    //             .GetMethods()
    //             .Where(x => x.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(UnicornOneWayAttribute)) is not null);
    //
    //         foreach (var interfaceMethod in interfaceMethods)
    //         {
    //             var controllerMethod = controller
    //                 .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
    //                 .Where(x => x.CustomAttributes.Any(x => x.AttributeType == typeof(NonActionAttribute)))
    //                 .Single(x => x.Name == interfaceMethod.Name && x.GetParameters().Length == interfaceMethod.GetParameters().Length);
    //
    //             configurations.Add(new OneWayMethodConfiguration
    //             {
    //                 InterfaceMethod = interfaceMethod,
    //                 ControllerMethod = controllerMethod
    //             });
    //         }
    //     }
    //
    //     return configurations;
    // }

    private static bool IsUnicornAssembly(Assembly assembly) => IsUnicornAssemblyName(assembly.GetName());

    private static bool IsUnicornAssemblyName(AssemblyName assemblyName) =>
        assemblyName.Name!.Contains("Unicorn", StringComparison.OrdinalIgnoreCase);

    private static IEnumerable<string> GetAssemblyFilesFromCurrentDirectory()
    {
        var path = Path.GetDirectoryName(typeof(AssemblyScanner).Assembly.Location) ?? string.Empty;
        var f = Directory.GetFiles(path).Where(fileName => fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));

        return f;
    }

    private static MetadataLoadContext GetMetadataLoadContext()
    {
        // Important: all assemblies (besides current and core assemblies) of referenced projects whose types
        // will be used in reflection must be added or exception will be thrown about not loaded assembly.
        // Right now only 2 projects are used in reflection:
        // Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc and
        // Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Http

        var currentAssembly = typeof(AssemblyScanner).Assembly;
        var coreAssembly = typeof(object).Assembly;
        var httpServiceMarkerAssembly = typeof(UnicornRestServiceMarkerAttribute).Assembly;
        var grpcClientMarkerAssembly = typeof(UnicornGrpcClientMarkerAttribute).Assembly;

        var filePaths = new[]
        {
            currentAssembly.Location,
            coreAssembly.Location,
            httpServiceMarkerAssembly.Location,
            grpcClientMarkerAssembly.Location,
        };

        var runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");

        // Create the list of assembly paths consisting of runtime assemblies and the inspected assembly.
        var paths = new List<string>(runtimeAssemblies.Union(filePaths));

        return new MetadataLoadContext(new PathAssemblyResolver(paths));

    }
}
