using Grpc.Net.Client;
using GrpcService1;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;

namespace Unicorn.GrpcService.SDK.Grpc.Clients;

[UnicornGrpcClientMarker]
public interface IGreeterProtoClient
{
    Task<HelloReply> SayHelloAsync(HelloRequest request);
}

public class GreeterProtoClient : BaseGrpcClient, IGreeterProtoClient
{
    private Greeter.GreeterClient? _client;

    protected override string GrpcServiceName => "GreeterProtoService";

    public GreeterProtoClient(IGrpcServiceClientFactory factory)
        : base(factory)
    {
    }

    public async Task<HelloReply> SayHelloAsync(HelloRequest request) =>
        await Factory.CallAsync(GrpcServiceName, c => GetClient(c)!.SayHelloAsync(request));

    private Greeter.GreeterClient? GetClient(GrpcChannel channel) => _client ??= new Greeter.GreeterClient(channel);
}
