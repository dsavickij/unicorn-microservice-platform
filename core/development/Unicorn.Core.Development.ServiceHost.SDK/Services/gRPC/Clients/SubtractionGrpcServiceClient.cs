using Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Protos;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;

namespace Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Clients;

[UnicornGrpcClientMarker]
public interface ISubtractionGrpcServiceClient
{
    Task<int> SubtractAsync(int first, int second);
    Task<decimal> SubtractDecimalAsync(decimal first, decimal second);
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

    public async Task<decimal> SubtractDecimalAsync(decimal first, decimal second)
    {
        var response = await _factory.CallAsync(
            c => new SubtractionGrpcService.SubtractionGrpcServiceClient(c).SubtractDecimalAsync(
                new DecimalSubtractionRequest { FirstOperand = first, SecondOperand = second }));

        return response.Result;
    }
}
