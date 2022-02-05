using Grpc.Core;
using Unicorn.Core.Development.ServiceHost.Grpc.Protos.DivisionGrpcService;

// [Authorize]
public class DivisionGrpcService : Unicorn.Core.Development.ServiceHost.Grpc.Protos.DivisionGrpcService.DivisionGrpcService.DivisionGrpcServiceBase
{
    public override Task<DivisionResponse> Divide(DivisionRequest request, ServerCallContext context)
    {
        var result = request.FirstOperand / request.SecondOperand;
        var response = new DivisionResponse { Result = result };

        return Task.FromResult(response);
    }
}