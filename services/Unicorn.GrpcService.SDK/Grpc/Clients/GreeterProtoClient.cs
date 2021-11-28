using Grpc.Net.Client;
using GrpcService1;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc;
using Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc.Contracts;

namespace Unicorn.GrpcService.SDK.Grpc.Clients;

public interface IGreeterProtoClient
{
    Task<HelloReply> SayHelloAsync(HelloRequest request);
}

public class GreeterProtoClient : BaseGrpcClient, IGreeterProtoClient
{
    private Greeter.GreeterClient? _client;

    protected override string GrpcServiceName => "GreeterProtoClient";

    public GreeterProtoClient(IGrpcClientFactory factory)
        : base(factory)
    {
    }

    public async Task<HelloReply> SayHelloAsync(HelloRequest request) =>
        await Factory.Call(GrpcServiceName, c => GetClient(c)!.SayHelloAsync(request));

    private Greeter.GreeterClient? GetClient(GrpcChannel channel) => _client ??= new Greeter.GreeterClient(channel);
}
