using Grpc.Core;
using Grpc.Net.Client;

namespace Playground.Common.SDK.Abstractions;

public interface IGrpcClientFactory
{
    Task<T> Call<T>(string grpcClientName, Func<GrpcChannel, AsyncUnaryCall<T>> p);
}
