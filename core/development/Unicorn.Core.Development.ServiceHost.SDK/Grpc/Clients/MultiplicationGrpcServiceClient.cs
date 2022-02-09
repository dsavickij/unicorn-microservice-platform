using Unicorn.Core.Development.ServiceHost.SDK.Grpc.Protos;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;

namespace Unicorn.Core.Development.ServiceHost.SDK.Grpc.Clients;

[UnicornGrpcClientMarker]
public interface IMultiplicationGrpcServiceClient
{
    Task<int> MultiplyAsync(int first, int second);
}

public class MultiplicationGrpcServiceClient : BaseGrpcClient, IMultiplicationGrpcServiceClient
{
    private readonly IGrpcServiceClientFactory _factory;

    public MultiplicationGrpcServiceClient(IGrpcServiceClientFactory factory) : base(factory)
    {
        _factory = factory;
    }

    public async Task<int> MultiplyAsync(int first, int second)
    {
        var response = await _factory.CallAsync(
            c => new MultiplicationGrpcService.MultiplicationGrpcServiceClient(c).MultiplyAsync(
               new MultiplicationRequest { FirstOperand = first, SecondOperand = second }));

        return response.Result;
    }
}
