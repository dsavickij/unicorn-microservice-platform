using Grpc.Core;
using Grpc.Net.Client;

namespace Unicorn.Core.Infrastructure.SDK.ServiceCommunication.Grpc.Contracts;

public interface IGrpcClientFactory
{
    Task<T> Call<T>(string grpcClientName, Func<GrpcChannel, AsyncUnaryCall<T>> p);
}
