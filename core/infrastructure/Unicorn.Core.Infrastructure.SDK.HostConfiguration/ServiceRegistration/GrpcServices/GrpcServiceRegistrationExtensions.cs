using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.Common;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc.Contracts;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.GrpcServices;

internal static class GrpcServiceRegistrationExtensions
{
    public static void AddGrpcServices(this IServiceCollection services)
    {
        services.AddTransient<IGrpcClientFactory, GrpcClientFactory>();
        services.AddSingleton<IGrpcServiceConfigurationProvider, GrpcServiceConfigurationProvider>();

        foreach (var (grpcInterface, grpcImpl) in GetGrpcServiceRegistrationTypePairs())
        {
            services.AddTransient(grpcInterface, grpcImpl);
        }
    }

    private static IEnumerable<(Type grpcInterface, Type grpcImpl)> GetGrpcServiceRegistrationTypePairs()
    {
        var baseGrpcClientImplName = typeof(BaseGrpcClient).AssemblyQualifiedName;
        var pairs = new List<(Type, Type)>();

        foreach (var name in AssemblyInspector.GetServiceInterfaceNamesWithAttributeOfType<UnicornGrpcClientMarker>())
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
}