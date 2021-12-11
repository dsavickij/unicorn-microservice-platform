using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Unicorn.Core.Development.ServiceHost.Protos;
using static Unicorn.Core.Development.ServiceHost.Protos.MyGrpcService;

[Authorize]
public class MyGrpcService : MyGrpcServiceBase
{
    public override Task<MultiplicationResponse> Multiply(MultiplicationRequest request, ServerCallContext context)
    {
        var result = request.FirstOperand * request.SecondOperand;
        var response = new MultiplicationResponse { Result = result };

        return Task.FromResult(response);
    }
}