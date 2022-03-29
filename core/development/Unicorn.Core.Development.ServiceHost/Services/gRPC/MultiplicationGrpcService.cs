using Grpc.Core;
using Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Protos;

// [Authorize]
public class MultiplicationGrpcService : Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Protos.MultiplicationGrpcService.MultiplicationGrpcServiceBase
{
    public override Task<MultiplicationResponse> Multiply(MultiplicationRequest request, ServerCallContext context)
    {
        var result = request.FirstOperand * request.SecondOperand;
        var response = new MultiplicationResponse { Result = result };

        return Task.FromResult(response);
    }

    public override async Task SequencePowerOfTwo(
        SequencePowerOfTwoRequest request,
        IServerStreamWriter<MultiplicationResponse> responseStream,
        ServerCallContext context)
    {
        foreach (var number in request.Sequence)
        {
            var powerOfTwo = (int)Math.Pow(number, 2);
            await responseStream.WriteAsync(new MultiplicationResponse { Result = powerOfTwo });
        }
    }
}