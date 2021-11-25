using Microsoft.Extensions.DependencyInjection;

namespace Unicorn.GrpcService.SDK.Grpc.Clients;

public static class GreeterProtoClientRegistrationExtensions
{
    public static void AddGreeterProtoClient(this IServiceCollection services)
    {
        services.AddTransient<IGreeterProtoClient, GreeterProtoClient>();
    }
}
