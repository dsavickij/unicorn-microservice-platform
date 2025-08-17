using Grpc.Core;
using Grpc.Net.Client;

namespace Unicorn.Core.Infrastructure.Communication.SDK.TwoWay.gRPC.Contracts;

public interface IGrpcServiceClientFactory
{
    Task<T> CallAsync<T>(Func<GrpcChannel, AsyncUnaryCall<T>> grpcServiceMethod);
    
    IAsyncEnumerable<T> GetResponseStreamAsync<T>(
        Func<GrpcChannel, AsyncServerStreamingCall<T>> grpcServiceMethod,
        CancellationToken token);
    
    Task<TResponse> GetRequestStreamAsync<TRequest, TResponse>(
        Func<GrpcChannel, AsyncClientStreamingCall<TRequest, TResponse>> grpcServiceMethod,
        IAsyncEnumerable<TRequest> requests,
        CancellationToken token);
}
