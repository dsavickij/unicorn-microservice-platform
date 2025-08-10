using Grpc.Core;
using Unicorn.Core.Development.Server.SDK.Services.gRPC.Protos;

// [Authorize]
namespace Unicorn.Core.Development.Server.Services.gRPC;

public class DivisionGrpcService : Unicorn.Core.Development.Server.SDK.Services.gRPC.Protos.DivisionGrpcService.DivisionGrpcServiceBase
{
    public override Task<DivisionResponse> Divide(DivisionRequest request, ServerCallContext context)
    {
        var result = request.FirstOperand / request.SecondOperand;
        var response = new DivisionResponse { Result = result };

        return Task.FromResult(response);
    }
}