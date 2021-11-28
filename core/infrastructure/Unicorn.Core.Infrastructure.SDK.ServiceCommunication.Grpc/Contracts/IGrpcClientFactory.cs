using Grpc.Core;
using Grpc.Net.Client;

namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc.Contracts;

public interface IGrpcClientFactory
{
    Task<T> CallAsync<T>(string grpcServiceName, Func<GrpcChannel, AsyncUnaryCall<T>> grpcServiceMethod);
}
