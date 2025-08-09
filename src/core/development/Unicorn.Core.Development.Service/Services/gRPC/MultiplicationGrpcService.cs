using Grpc.Core;
using Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Protos;

// [Authorize]
namespace Unicorn.Core.Development.Service.Services.gRPC;

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

    public override async Task<MultiplicationSequenceSumResponse> GetMultiplicationSequenceSum(
        IAsyncStreamReader<MultiplicationSequenceSumRequest> requestStream, ServerCallContext context)
    {
        var sum = 0;

        while (await requestStream.MoveNext())
        {
            var message = requestStream.Current;
            var request = new MultiplicationRequest
            {
                FirstOperand = message.FirstOperand,
                SecondOperand = message.SecondOperand
            };

            var response = await Multiply(request, context);
            sum += response.Result;
        }

        return new MultiplicationSequenceSumResponse { Result = sum };
    }
}