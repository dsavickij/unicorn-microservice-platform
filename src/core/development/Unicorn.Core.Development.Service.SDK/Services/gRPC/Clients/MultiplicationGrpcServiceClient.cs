using Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Protos;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.gRPC;
using Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.gRPC.Contracts;

namespace Unicorn.Core.Development.ServiceHost.SDK.Services.gRPC.Clients;

[UnicornGrpcClientMarker]
public interface IMultiplicationGrpcServiceClient
{
    Task<int> MultiplyAsync(int first, int second);
    IAsyncEnumerable<int> GetSequencePowerOfTwoAsync(IEnumerable<int> sequence, CancellationToken token);
    Task<int> GetMultiplicationSequnceSumAsync(IAsyncEnumerable<(int, int)> producer, CancellationToken token);
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

        var responseStream = _factory.GetResponseStreamAsync(
            c => new MultiplicationGrpcService.MultiplicationGrpcServiceClient(c).SequencePowerOfTwo(
                request, cancellationToken: token),
            token);

        await foreach (var response in responseStream) yield return response.Result;
    }

    public async Task<int> GetMultiplicationSequnceSumAsync(IAsyncEnumerable<(int, int)> provider, CancellationToken token)
    {
        var response = await _factory.GetRequestStreamAsync<MultiplicationSequenceSumRequest, MultiplicationSequenceSumResponse>(
            c => new MultiplicationGrpcService.MultiplicationGrpcServiceClient(c).GetMultiplicationSequenceSum(),
            GetWrappedInRequests(),
            token);

        return response.Result;

        async IAsyncEnumerable<MultiplicationSequenceSumRequest> GetWrappedInRequests()
        {
            await foreach (var (firstOperand, secondOperand) in provider)
            {
                yield return new MultiplicationSequenceSumRequest
                {
                    FirstOperand = firstOperand,
                    SecondOperand = secondOperand
                };
            }
        }
    }
}