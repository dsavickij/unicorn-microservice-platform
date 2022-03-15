using Grpc.Core;
using Unicorn.Templates.Service.SDK.Services.gRPC.Protos;

namespace Unicorn.Templates.Service.Services.gRPC;

public class YOUR_SERVICE_NAMEGrpcService : SDK.Services.gRPC.Protos.YOUR_SERVICE_NAMEGrpcService.YOUR_SERVICE_NAMEGrpcServiceBase
{
    public override Task<DecimalSubtractionResponse> SubtractDecimal(DecimalSubtractionRequest request, ServerCallContext context)
    {
        var result = request.FirstOperand - request.SecondOperand;
        var response = new DecimalSubtractionResponse { Result = result };

        return Task.FromResult(response);
    }
}
