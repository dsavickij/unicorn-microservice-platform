using Unicorn.Core.Infrastructure.Communication.Grpc.SDK;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;
using Unicorn.Templates.Service.SDK.Services.gRPC.Protos;

namespace Unicorn.Core.Development.ServiceHost.SDK.Grpc.Clients;

[UnicornGrpcClientMarker]
public interface IYOUR_SERVICE_NAMEGrpcServiceClient
{
    Task<decimal> SubtractDecimalAsync(decimal first, decimal second);
}

public class YOUR_SERVICE_NAMEGrpcServiceClient : BaseGrpcClient, IYOUR_SERVICE_NAMEGrpcServiceClient
{
    private readonly IGrpcServiceClientFactory _factory;

    public YOUR_SERVICE_NAMEGrpcServiceClient(IGrpcServiceClientFactory factory)
        : base(factory)
    {
        _factory = factory;
    }

    public async Task<decimal> SubtractDecimalAsync(decimal first, decimal second)
    {
        var response = await _factory.CallAsync(
            c => new YOUR_SERVICE_NAMEGrpcService.YOUR_SERVICE_NAMEGrpcServiceClient(c).SubtractDecimalAsync(
                new DecimalSubtractionRequest { FirstOperand = first, SecondOperand = second }));

        return response.Result;
    }
}
