using Grpc.Core;
using Grpc.Net.Client;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.gRPC.Contracts;
using Unicorn.Core.Infrastructure.Security.IAM.AuthenticationContext;

namespace Unicorn.Core.Infrastructure.HostConfiguration.SDK.ServiceRegistration.GrpcServiceClients;

internal class GrpcServiceClientFactory : IGrpcServiceClientFactory
{
    private readonly IGrpcServiceConfigurationProvider _cfgProvider;

    public GrpcServiceClientFactory(IGrpcServiceConfigurationProvider configurationProvider)
    {
        _cfgProvider = configurationProvider;
    }

    public async Task<T> CallAsync<T>(Func<GrpcChannel, AsyncUnaryCall<T>> grpcServiceMethod)
    {
        var cfg = await _cfgProvider.GetGrpcServiceConfigurationAsync(grpcServiceMethod.Method);
        using var channel = GetChannel(cfg.BaseUrl);

        return await grpcServiceMethod(channel);
    }

    public async IAsyncEnumerable<T> GetResponseStreamAsync<T>(
        Func<GrpcChannel, AsyncServerStreamingCall<T>> grpcServiceMethod, CancellationToken token)
    {
        var cfg = await _cfgProvider.GetGrpcServiceConfigurationAsync(grpcServiceMethod.Method);
        using var channel = GetChannel(cfg.BaseUrl);

        using var stream = grpcServiceMethod.Invoke(channel);

        while (await stream.ResponseStream.MoveNext(token))
        {
            yield return stream.ResponseStream.Current;
        }
    }

    public async Task<TResponse> GetRequestStreamAsync<TRequest, TResponse>(
        Func<GrpcChannel, AsyncClientStreamingCall<TRequest, TResponse>> grpcServiceMethod,
        IAsyncEnumerable<TRequest> requests,
        CancellationToken token)
    {
        var cfg = await _cfgProvider.GetGrpcServiceConfigurationAsync(grpcServiceMethod.Method);
        using var channel = GetChannel(cfg.BaseUrl);

        using var stream = grpcServiceMethod.Invoke(channel);

        await foreach (var req in requests)
        {
            await stream.RequestStream.WriteAsync(req);
        }

        await stream.RequestStream.CompleteAsync();

        return await stream;
    }

    private GrpcChannel GetChannel(string baseUrl)
    {
        if (string.IsNullOrEmpty(UnicornOperationContext.AccessToken))
        {
            // if no access token exist, it is assummed non-SSL channel needs to be used
            var httpHandler = new HttpClientHandler
            {
                // Return 'true' to allow certificates that are untrusted/invalid
                // TODO: investigate how to work with SSL while running on Docker
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
