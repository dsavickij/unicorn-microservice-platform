using Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Protos;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK;
using Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;

namespace Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Clients;

[UnicornGrpcClientMarker]
public interface IMultiplicationGrpcServiceClient
{
    Task<int> MultiplyAsync(int first, int second);
    IAsyncEnumerable<int> GetSequencePowerOfTwoAsync(IEnumerable<int> sequence, CancellationToken token);
}

public class MultiplicationGrpcServiceClient : BaseGrpcClient, IMultiplicationGrpcServiceClient
{
    private readonly IGrpcServiceClientFactory _factory;

    public MultiplicationGrpcServiceClient(IGrpcServiceClientFactory factory) : base(factory)
    {
        _factory = factory;
    }

    public async Task<int> MultiplyAsync(int first, int second)
    {
        var response = await _factory.CallAsync(
            c => new MultiplicationGrpcService.MultiplicationGrpcServiceClient(c).MultiplyAsync(
               new MultiplicationRequest { FirstOperand = first, SecondOperand = second }));

        return response.Result;
    }

    public async IAsyncEnumerable<int> GetSequencePowerOfTwoAsync(IEnumerable<int> sequence, CancellationToken token)
    {
        var request = new SequencePowerOfTwoRequest();
        request.Sequence.AddRange(sequence);

        var itemStream = _factory.GetItemStreamAsync(
            c => new MultiplicationGrpcService.MultiplicationGrpcServiceClient(c).SequencePowerOfTwo(request, cancellationToken: token), token);

        await foreach (var item in itemStream)
        {
            yield return item.Result;
        }
    }
}
