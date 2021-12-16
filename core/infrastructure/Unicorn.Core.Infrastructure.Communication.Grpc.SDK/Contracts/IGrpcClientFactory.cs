using Grpc.Core;
using Grpc.Net.Client;

namespace Unicorn.Core.Infrastructure.Communication.Grpc.SDK.Contracts;

public interface IGrpcServiceClientFactory
{
    Task<T> CallAsync<T>(string grpcServiceName, Func<GrpcChannel, AsyncUnaryCall<T>> grpcServiceMethod);
}
