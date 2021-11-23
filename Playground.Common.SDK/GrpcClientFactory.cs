using Grpc.Core;
using Grpc.Net.Client;
using Playground.Common.SDK.Abstractions;

namespace Playground.Common.SDK;

public class GrpcClientFactory : IGrpcClientFactory
{
    private readonly IGrpcClientConfigurationProvider _cfgProvider;

    public GrpcClientFactory(IGrpcClientConfigurationProvider configurationProvider)
    {
        _cfgProvider = configurationProvider;
    }

    public async Task<T> Call<T>(string grpcServiceName, Func<GrpcChannel, AsyncUnaryCall<T>> grpcClientEndpointCall)
    {
        var cfg = await _cfgProvider.GetGrpcServiceConfigurationAsync(grpcServiceName);
        var channel = GrpcChannel.ForAddress(cfg.BaseUrl);

        return await grpcClientEndpointCall(channel);
    }
}