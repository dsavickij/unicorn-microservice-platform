using Playground.Common.SDK.Abstractions;

namespace GrpcService1.SDK.GrpcClients;

public interface IGreeterProtoClient
{
    Task<HelloReply> SayHelloAsync(HelloRequest request);
}

public class GreeterProtoClient : BaseGrpcClient, IGreeterProtoClient
{
    private Greeter.GreeterClient _client;

    private const string _grpcClientName = "GreeterProtoClient";

    protected override string GrpcServiceName => "GreeterProtoClient";

    public GreeterProtoClient(IGrpcClientFactory factory) : base(factory) { }

    public async Task<HelloReply> SayHelloAsync(HelloRequest request) =>
        await Factory.Call(_grpcClientName, c => (_client ??= new Greeter.GreeterClient(c)).SayHelloAsync(request));
}