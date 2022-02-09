using Grpc.Core;
using Unicorn.Core.Development.ServiceHost.SDK.Grpc.Protos;

// [Authorize]
public class MultiplicationGrpcService : Unicorn.Core.Development.ServiceHost.SDK.Grpc.Protos.MultiplicationGrpcService.MultiplicationGrpcServiceBase
{
    public override Task<MultiplicationResponse> Multiply(MultiplicationRequest request, ServerCallContext context)
    {
        var result = request.FirstOperand * request.SecondOperand;
        var response = new MultiplicationResponse { Result = result };

        return Task.FromResult(response);
    }
}