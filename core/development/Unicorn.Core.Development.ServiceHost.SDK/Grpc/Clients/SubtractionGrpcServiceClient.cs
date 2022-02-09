using Unicorn.Core.Development.ServiceHost.SDK.Grpc.Protos;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;

namespace Unicorn.Core.Development.ServiceHost.SDK.Grpc.Clients;

[UnicornGrpcClientMarker]
public interface ISubtractionGrpcServiceClient
{
    Task<int> SubtractAsync(int first, int second);
}

public class SubtractionGrpcServiceClient : BaseGrpcClient, ISubtractionGrpcServiceClient
{
    private readonly IGrpcServiceClientFactory _factory;

    public SubtractionGrpcServiceClient(IGrpcServiceClientFactory factory) : base(factory)
    {
        _factory = factory;
    }

    public async Task<int> SubtractAsync(int first, int second)
    {
        var response = await _factory.CallAsync(
            c => new SubtractionGrpcService.SubtractionGrpcServiceClient(c).SubtractAsync(
               new SubtractionRequest { FirstOperand = first, SecondOperand = second }));

        return response.Result;
    }
}
