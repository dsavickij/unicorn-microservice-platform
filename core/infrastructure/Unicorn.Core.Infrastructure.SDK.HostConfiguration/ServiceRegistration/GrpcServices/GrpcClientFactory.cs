using Grpc.Core;
using Grpc.Net.Client;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc.Contracts;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.GrpcServices;

internal class GrpcClientFactory : IGrpcClientFactory
{
    private readonly IGrpcServiceConfigurationProvider _cfgProvider;

    public GrpcClientFactory(IGrpcServiceConfigurationProvider configurationProvider)
    {
        _cfgProvider = configurationProvider;
    }

    public async Task<T> Call<T>(string grpcServiceName, Func<GrpcChannel, AsyncUnaryCall<T>> grpcMethod)
    {
        var cfg = await _cfgProvider.GetGrpcServiceConfigurationAsync(grpcServiceName);
        var channel = GrpcChannel.ForAddress(cfg.BaseUrl);

        return await grpcMethod(channel);
    }
}