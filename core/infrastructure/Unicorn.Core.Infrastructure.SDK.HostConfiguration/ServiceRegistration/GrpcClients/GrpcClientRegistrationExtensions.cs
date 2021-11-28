using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc.Contracts;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.GrpcClients;

internal static class GrpcClientRegistrationExtensions
{
    public static void AddGrpcClients(this IServiceCollection services)
    {
        services.AddTransient<IGrpcClientFactory, GrpcClientFactory>();
        services.AddSingleton<IGrpcServiceConfigurationProvider, GrpcServiceConfigurationProvider>();

        foreach (var (grpcClientInterface, grpcClientImpl) in GetGrpcServiceRegistrationTypePairs())
        {
            services.AddTransient(grpcClientInterface, grpcClientImpl);
        }
    }

    private static IEnumerable<(Type grpcInterface, Type grpcImpl)> GetGrpcServiceRegistrationTypePairs()
    {
        var baseGrpcClientImplName = typeof(BaseGrpcClient).AssemblyQualifiedName;
        var pairs = new List<(Type, Type)>();

        foreach (var name in AssemblyInspector.GetInterfaceNamesWithAttribute<UnicornGrpcClientMarkerAttribute>())
        {
            var grpcInterfaceType = Guard.Against.Null(Type.GetType(name, true), name);

            var grpcImplType = grpcInterfaceType!.Assembly
                .GetExportedTypes()
                .FirstOrDefault(t => t.IsClass && !t.IsAbstract && t.BaseType?.AssemblyQualifiedName == baseGrpcClientImplName);

            if (grpcImplType is not null)
            {
                pairs.Add((grpcInterfaceType, grpcImplType));
            }
            else
            {
                throw new ArgumentException($"Grpc client interface '{name}' does not have implementation " +
                    $"in assembly '{grpcInterfaceType.Assembly.FullName}'");
            }
        }

        return pairs;
    }
}
