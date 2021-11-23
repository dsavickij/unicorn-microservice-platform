using Microsoft.Extensions.DependencyInjection;

namespace GrpcService1.SDK.GrpcClients;

public static class GreeterProtoClientRegistrationExtensions
{
    public static void AddGreeterProtoClient(this IServiceCollection services)
    {
        services.AddTransient<IGreeterProtoClient, GreeterProtoClient>();
    }
}
