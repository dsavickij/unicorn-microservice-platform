using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.GrpcServiceClients;

internal static class GrpcServiceClientRegistrationExtensions
{
    public static void AddGrpcClients(this IServiceCollection services)
    {
        services.AddTransient<IGrpcServiceClientFactory, GrpcServiceClientFactory>();
        services.AddSingleton<IGrpcServiceConfigurationProvider, GrpcServiceConfigurationProvider>();

        foreach (var (grpcClientInterface, grpcClientImpl) in GetGrpcServiceClientRegistrationTypePairs())
        {
            services.AddTransient(grpcClientInterface, grpcClientImpl);
        }
    }

    private static IEnumerable<(Type grpcInterface, Type grpcImpl)> GetGrpcServiceClientRegistrationTypePairs()
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
