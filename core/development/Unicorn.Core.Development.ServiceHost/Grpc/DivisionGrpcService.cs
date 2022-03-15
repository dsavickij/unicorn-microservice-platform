using Grpc.Core;
using Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Protos;

// [Authorize]
public class DivisionGrpcService : Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Protos.DivisionGrpcService.DivisionGrpcServiceBase
{
    public override Task<DivisionResponse> Divide(DivisionRequest request, ServerCallContext context)
    {
        var result = request.FirstOperand / request.SecondOperand;
        var response = new DivisionResponse { Result = result };

        return Task.FromResult(response);
    }
}