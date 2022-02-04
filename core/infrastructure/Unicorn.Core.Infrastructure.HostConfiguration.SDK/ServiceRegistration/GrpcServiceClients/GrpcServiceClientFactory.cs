using Grpc.Core;
using Grpc.Net.Client;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;
using Unicorn.Core.Infrastructure.Security.IAM.AuthenticationContext;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.GrpcServiceClients;

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
        using var channel = GetChannel(cfg.BaseUrl);

        return await grpcServiceMethod(channel);
    }

    private GrpcChannel GetChannel(string baseUrl)
    {
        if (string.IsNullOrEmpty(UnicornOperationContext.AccessToken))
        {
            // if no access token exist, it is assummed non-SSL channel need to be used
            var httpHandler = new HttpClientHandler
            {
                // Return 'true' to allow certificates that are untrusted/invalid
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            return GrpcChannel.ForAddress(baseUrl, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.SecureSsl,
                HttpHandler = httpHandler
            });
        }

        // SSL channel
        return GrpcChannel.ForAddress(baseUrl, new GrpcChannelOptions
        {
            Credentials = ChannelCredentials.Create(new SslCredentials(), GetCallCredentials()),
        });
    }

    private CallCredentials GetCallCredentials()
    {
        return CallCredentials.FromInterceptor((context, metadata) =>
        {
            metadata.Add("Authorization", $"Bearer {UnicornOperationContext.AccessToken}");
            return Task.CompletedTask;
        });
    }
}
