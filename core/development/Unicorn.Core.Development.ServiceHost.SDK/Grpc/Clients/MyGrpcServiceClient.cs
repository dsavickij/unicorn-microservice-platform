using Unicorn.Core.Development.ServiceHost.Protos;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;

namespace Unicorn.Core.Development.ServiceHost.SDK.Grpc.Clients;

[UnicornGrpcClientMarker]
public interface IMyGrpcServiceClient
{
    Task<int> Multiply(int first, int second);
}

public class MyGrpcServiceClient : BaseGrpcClient, IMyGrpcServiceClient
{
    private readonly IGrpcServiceClientFactory _factory;

    public MyGrpcServiceClient(IGrpcServiceClientFactory factory) : base(factory)
    {
        _factory = factory;
    }

    protected override string GrpcServiceName => "MyGrpcService";

    public async Task<int> Multiply(int first, int second)
    {
        var result = await _factory.CallAsync(
           GrpcServiceName, c => new MyGrpcService.MyGrpcServiceClient(c).MultiplyAsync(
               new Protos.MultiplicationRequest { FirstOperand = first, SecondOperand = second }));

        return result.Result;
    }
}
