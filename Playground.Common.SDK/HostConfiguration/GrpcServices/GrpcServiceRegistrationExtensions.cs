using Microsoft.Extensions.DependencyInjection;
using Playground.Common.SDK.Abstractions;
using System.Reflection;

namespace Playground.Common.SDK.HostConfiguration.GrpcServices;

internal static class GrpcServiceRegistrationExtensions
{
    public static void AddGrpcServices(this IServiceCollection services)
    {
        services.AddTransient<IGrpcClientFactory, GrpcClientFactory>();
        services.AddSingleton<IGrpcClientConfigurationProvider, GrpcClientConfigurationProvider>();

        foreach (var (grpcInterface, grpcImpl) in GetGrpcServiceRegistrationTypePairs())
        {
            services.AddTransient(grpcInterface, grpcImpl);
        }
    }

    private static IEnumerable<(Type grpcInterface, Type grpcImpl)> GetGrpcServiceRegistrationTypePairs()
    {
        var baseGrpcClientImplName = typeof(BaseGrpcClient).AssemblyQualifiedName;
        var pairs = new List<(Type, Type)>();

        foreach (var name in GetGrpcServiceInterfaceNames())
        {
            var grpcInterfaceType = Type.GetType(name, true) ?? throw new ArgumentNullException(name);

            var grpcImplType = grpcInterfaceType!.Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.AssemblyQualifiedName == baseGrpcClientImplName)
                .FirstOrDefault();

            if (grpcImplType is not null)
            {
                pairs.Add((grpcInterfaceType, grpcImplType));
            }
            else throw new Exception($"Grpc client interface '{name}' does not have implementation " +
                $"in assembly '{grpcInterfaceType.Assembly.FullName}'");
        }

        return pairs;
    }

    private static MetadataLoadContext GetMedataLoadContext()
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var coreAssembly = typeof(object).Assembly;
        var grpcClientMarkerAssembly = typeof(GrpcClientMarker).Assembly;

        var filePathes = new[] { currentAssembly.Location, coreAssembly.Location, grpcClientMarkerAssembly.Location };

        return new MetadataLoadContext(new PathAssemblyResolver(filePathes), coreAssembly.GetName().Name);
    }

    private static IEnumerable<string> GetGrpcServiceInterfaceNames()
    {
        var grpcClientMarkerName = typeof(GrpcClientMarker).AssemblyQualifiedName;
        var grpcInterfaceNames = new List<string>();

        using var ctx = GetMedataLoadContext();

        foreach (var file in GetAssemblyFilesFromCurrentDirectory())
        {
            var assembly = ctx.LoadFromAssemblyPath(file);

            if (IsAssemblyPlaygroundServiceAssembly(assembly))
            {
                var grpcServiceInterfacesNames = assembly
                    .GetExportedTypes()
                    .Where(t => t.IsInterface && t.GetCustomAttributesData().Any(
                        data => data.AttributeType.AssemblyQualifiedName == grpcClientMarkerName))
                    .Select(t => t.AssemblyQualifiedName ?? string.Empty);

                grpcInterfaceNames.AddRange(grpcServiceInterfacesNames);
            }
        }

        return grpcInterfaceNames;
    }

    private static bool IsAssemblyPlaygroundServiceAssembly(Assembly assembly) =>
        assembly.FullName!.Contains("Playground");

    private static IEnumerable<string> GetAssemblyFilesFromCurrentDirectory()
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        return Directory.GetFiles(path).Where(fileName => fileName.EndsWith(".dll"));
    }
}