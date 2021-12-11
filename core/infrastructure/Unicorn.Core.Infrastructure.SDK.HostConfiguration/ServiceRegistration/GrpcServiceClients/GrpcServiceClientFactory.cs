using Grpc.Core;
using Grpc.Net.Client;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc.Contracts;
using Unicorn.Core.Infrastructure.Security.IAM.AuthenticationContext;

namespace Unicorn.Core.Infrastructure.SDK.HostConfiguration.ServiceRegistration.GrpcServiceClients;

internal class GrpcServiceClientFactory : IGrpcServiceClientFactory
{
    private readonly IGrpcServiceConfigurationProvider _cfgProvider;

    public GrpcServiceClientFactory(IGrpcServiceConfigurationProvider configurationProvider)
    {
        _cfgProvider = configurationProvider;
    }

    public async Task<T> CallAsync<T>(string grpcServiceName, Func<GrpcChannel, AsyncUnaryCall<T>> grpcServiceMethod)
    {
        var cfg = await _cfgProvider.GetGrpcServiceConfigurationAsync(grpcServiceName);
        var creds = GetCallredentials();

        var channel = GrpcChannel.ForAddress(cfg.BaseUrl, new GrpcChannelOptions
        {
            Credentials = ChannelCredentials.Create(new SslCredentials(), creds)
        });

        return await grpcServiceMethod(channel);
    }

    private CallCredentials GetCallredentials()
    {
        return CallCredentials.FromInterceptor((context, metadata) =>
        {
            if (!string.IsNullOrEmpty(UnicornOperationContext.AccessToken))
            {
                metadata.Add("Authorization", $"Bearer {UnicornOperationContext.AccessToken}");
            }

            return Task.CompletedTask;
        });
    }
}
