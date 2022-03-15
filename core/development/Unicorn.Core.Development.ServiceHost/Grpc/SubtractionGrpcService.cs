using Grpc.Core;
using Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Protos;

// [Authorize]
public class SubtractionGrpcService : Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Protos.SubtractionGrpcService.SubtractionGrpcServiceBase
{
    public override Task<SubtractionResponse> Subtract(SubtractionRequest request, ServerCallContext context)
    {
        var result = request.FirstOperand - request.SecondOperand;
        var response = new SubtractionResponse { Result = result };

        return Task.FromResult(response);
    }
}