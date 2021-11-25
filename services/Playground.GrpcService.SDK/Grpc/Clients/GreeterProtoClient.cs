using Playground.Core.Infrastructure.SDK.ServiceCommunication.Grpc;
using Playground.Core.Infrastructure.SDK.ServiceCommunication.Grpc.Contracts;

namespace GrpcService1.SDK.GrpcClients;

public interface IGreeterProtoClient
{
    Task<HelloReply> SayHelloAsync(HelloRequest request);
}

public class GreeterProtoClient : BaseGrpcClient, IGreeterProtoClient
{
    private Greeter.GreeterClient _client;

    protected override string GrpcServiceName => "GreeterProtoClient";

    public GreeterProtoClient(IGrpcClientFactory factory) : base(factory) { }

    public async Task<HelloReply> SayHelloAsync(HelloRequest request) =>
        await Factory.Call(GrpcServiceName, c => (_client ??= new Greeter.GreeterClient(c)).SayHelloAsync(request));
}